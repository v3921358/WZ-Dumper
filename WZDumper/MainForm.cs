using MapleLib.WzLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WzDumper {
    public partial class MainForm : Form {
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);
        [DllImport("kernel32.dll", EntryPoint = "CreateSymbolicLinkW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

        public MainForm() {
            InitializeComponent();
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(includePngMp3Box, "生成 XML 文件時同時提取 png 圖片及 mp3 檔案");
            string linkTypeText = "設置創建鏈接文件的方法\n" +
                "筆記: Symbolic 和 Hard 模式不能提取到遠端硬碟上.\n" +
                "方法:\n" +
                "Symbolic (推薦, 需要管理員權限, 當以管理員權限開啟時為預設)\n" +
                "Hard (當沒有以管理員權限開啟時為預設, falls back to Copy mode for files that have reached the link limit)\n" +
                "Copy (完全創建另一個副本, 提取到遠程硬碟時使用)";
            toolTip.SetToolTip(LinkTypeLabel, linkTypeText);
            toolTip.SetToolTip(LinkTypeComboBox, linkTypeText);
            toolTip.SetToolTip(includeVersionInFolderBox, "將版本添加到WZ文件夾的結尾 (例如 Base.wz_v81)");
            toolTip.SetToolTip(multiThreadCheckBox, "只有在提取資料夾時啟用 \n根據線程數量，同時提取多個WZ文件。");
            toolTip.SetToolTip(extractorThreadsNum, "設置一次要提取的WZ文件的最大數量.\n預設是可用的處理器最大線程數.");
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;
            extractorThreadsNum.Maximum = Environment.ProcessorCount;
            extractorThreadsNum.Value = Environment.ProcessorCount;
        }

        public bool IsError { get; set; }
        public bool IsFinished { get; set; } = true;
        public bool Exit { get; set; }
        public TextBox InfoTextBox { get { return Info; } }
        public CancellationTokenSource CancelSource { get; set; }
        public bool ShouldExtractMP3PNG => includePngMp3Box.Checked;
        public LinkType SelectedLinkType { get; set; }

        private bool IsElevated {
            get {
                bool isElevated;
                using (WindowsIdentity identity = WindowsIdentity.GetCurrent()) {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                return isElevated;
            }
        }

        private WzMapleVersion SelectedVersion {
            get {
                switch (MapleVersionComboBox.SelectedIndex) {
                    case 1:
                        return WzMapleVersion.GMS;
                    case 2:
                        return WzMapleVersion.EMS;
                    default:
                        return WzMapleVersion.CLASSIC;
                }
            }
        }

        private void SelectWzFile(object sender, EventArgs e) {
            var openFile = new OpenFileDialog { Title = "選擇檔案", Filter = "WZ Files|*.wz|WZ Image Files|*.img" };
            if (openFile.ShowDialog() == DialogResult.OK) {
                InputSelected(openFile.FileName);
            }
            openFile.Dispose();
        }

        private void InputSelected(string input) {
            WZFileTB.Text = input;
            versionBox.Text = String.Empty;
            toolStripStatusLabel1.Text = String.Empty;
            DumpWzButton.Enabled = WZFileTB.Text.Length > 0 && outputFolderTB.Text.Length > 0;
        }

        public void UpdateToolstripStatus(string status) {
            toolStripStatusLabel1.Text = status;
        }

        private static void UpdateTextBox(TextBox tb, string info, bool append) {
            if (append)
                tb.AppendText(info);
            else
                tb.Text = info;
        }

        private void DumpListWz(WzListFile file, string fName, string directory, DateTime startTime) {
            var error = false;
            TextWriter tw = new StreamWriter(directory + "\\List.txt");
            try {
                foreach (var listEntry in file.WzListEntries) {
                    tw.WriteLine(listEntry);
                }
            } catch (Exception e) {
                error = true;
                MessageBox.Show("錯誤發生: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (Directory.GetFiles(directory).Length == 0)
                    Directory.Delete(directory, true);
            } finally {
                tw.Dispose();
            }
            if (error) {
                UpdateTextBoxInfo(Info, "提取時發生錯誤 " + fName, true);
                UpdateToolstripStatus("提取時發生錯誤 " + fName);
            } else {
                var duration = DateTime.Now - startTime;
                UpdateTextBoxInfo(Info, "結束提取 " + fName + " in " + GetDurationAsString(duration), true);
                UpdateToolstripStatus("提取 " + fName + " 成功");
            }
        }

        private static string GetValidFolderName(string baseFolder, bool checkFileOnly) {
            var currFolder = baseFolder;
            var index = 1;
            if (checkFileOnly) {
                while (File.Exists(currFolder)) {
                    currFolder = baseFolder + "-" + index;
                    index++;
                }
            } else {
                while (Directory.Exists(currFolder) || File.Exists(currFolder)) {
                    currFolder = baseFolder + "-" + index;
                    index++;
                }
            }
            return currFolder;
        }

        private void getWzExtensionFiles(string path, List<WzFile> fileList) {
            FileInfo wzFileInfo = new FileInfo(path);
            string selFileName = Path.GetFileNameWithoutExtension(wzFileInfo.Name);
            var extFiles = Directory.GetFiles(wzFileInfo.DirectoryName, selFileName + "???.wz");
            foreach (string extFile in extFiles) {
                if (Regex.IsMatch(extFile, selFileName + "[0-9]{3}.wz$", RegexOptions.IgnoreCase))
                    fileList.Add(new WzFile(extFile, SelectedVersion));
            }
        }

        private void DumpFile(object sender, EventArgs e) {
            CheckOutputPath();
            UpdateToolstripStatus("處理中...");
            DisableButtons();
            var filePath = WZFileTB.Text;
            FileAttributes attr = File.GetAttributes(filePath);
            if (!attr.HasFlag(FileAttributes.Directory)) {
                var ext = Path.GetExtension(filePath);
                if (ext != null && String.Compare(ext, ".img", StringComparison.OrdinalIgnoreCase) == 0) {
                    DumpXmlFromWzImage(filePath);
                    return;
                }
                WzListFile listFile = null;
                WzFile regFile = null;
                try {
                    if (filePath.EndsWith("List.wz", StringComparison.CurrentCultureIgnoreCase)) {
                        listFile = new WzListFile(filePath, SelectedVersion);
                        listFile.ParseWzFile();
                    } else {
                        List<WzFile> s = new List<WzFile>();
                        getWzExtensionFiles(filePath, s);
                        regFile = new WzFile(filePath, SelectedVersion, s);
                        regFile.ParseWzFile();
                    }
                } catch (UnauthorizedAccessException) {
                    if (regFile != null) regFile.Dispose();
                    MessageBox.Show("請使用管理員權限重新開啟此程式.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    UpdateToolstripStatus("");
                    EnableButtons();
                    return;
                } catch (Exception ex) {
                    if (regFile != null) regFile.Dispose();
                    MessageBox.Show("發生錯誤，訊息: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateToolstripStatus("");
                    EnableButtons();
                    return;
                }
                if (listFile == null)
                    versionBox.Text = regFile.Version.ToString(CultureInfo.CurrentCulture);
                var fileName = Path.GetFileName(filePath);
                var extractDir = outputFolderTB.Text;
                var extractFolder = Path.Combine(extractDir, fileName);
                if (listFile == null && includeVersionInFolderBox.Checked)
                    extractFolder += "_v" + regFile.Version;
                if (File.Exists(extractFolder)) {
                    extractFolder = GetValidFolderName(extractFolder, true);
                }
                if (Directory.Exists(extractFolder)) {
                    var result = MessageBox.Show(extractFolder + " 已經存在.\r\n是否要覆蓋資料夾?\r\n提示: 點選 No 將會創建新資料夾.", "Folder Already Exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Cancel) {
                        if (regFile != null) regFile.Dispose();
                        UpdateToolstripStatus("");
                        EnableButtons();
                        return;
                    }
                    if (result != DialogResult.Yes) {
                        extractFolder = GetValidFolderName(extractFolder, false);
                    }
                }
                if (!Directory.Exists(extractFolder))
                    Directory.CreateDirectory(extractFolder);
                if (listFile != null) {
                    Info.AppendText("提取資料從 " + fileName + " 到 " + extractFolder + "...\r\n");
                } else if (includePngMp3Box.Checked) {
                    Info.AppendText("提取 MP3s, PNGs 和XMLs 從檔案 " + fileName + " 到 " + extractFolder + "...\r\n");
                } else {
                    Info.AppendText("提取 XML 從 " + fileName + " 到 " + extractFolder + "...\r\n");
                }
                if (listFile != null) {
                    DumpListWz(listFile, fileName, extractFolder, DateTime.Now);
                    listFile.Dispose();
                    EnableButtons();
                } else {
                    UpdateToolstripStatus("準備中...");
                    CancelSource = new CancellationTokenSource();
                    CreateSingleDumperThread(regFile, new WzXml(this, extractDir, new DirectoryInfo(extractFolder).Name, includePngMp3Box.Checked, SelectedLinkType), fileName);
                }
            } else {
                var allFiles = Directory.GetFiles(filePath, "*.wz");
                if (allFiles.Length != 0) {
                    string filesFound = "未找到WZ文件: ";
                    foreach (var fileName in allFiles) {
                        filesFound += Path.GetFileName(fileName) + ", ";
                    }
                    Info.AppendText(filesFound.Substring(0, filesFound.Length - 2) + "\r\n");
                    // allFiles = allFiles.Where(fileName => !Regex.IsMatch(fileName, "[0-9]{3}.wz$", RegexOptions.IgnoreCase)).ToArray(); ;
                    Array.Sort(allFiles, Compare);
                    CreateMultipleDumperThreads(filePath, allFiles, outputFolderTB.Text);
                } else {
                    MessageBox.Show("所選擇的資料夾不包含Wz檔案. 請選擇其他資料夾.", "No WZ Files Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateToolstripStatus("");
                    EnableButtons();
                }
            }
        }

        private void DumpXmlFromWzImage(string path) {
            var fName = Path.GetFileName(path);
            if (fName == null)
                return;
            FileStream fStream;
            WzImage img;
            try {
                fStream = File.Open(path, FileMode.Open);
                img = new WzImage(fName, fStream, SelectedVersion);
                img.ParseImage();
            } catch (IOException ex) {
                MessageBox.Show("無法讀取檔案: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } catch (ArgumentException) {
                MessageBox.Show("請選擇Wz影像檔e.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } catch (UnauthorizedAccessException) {
                MessageBox.Show("Please re-run this program as an administrator to be able to dump files that are not in the OS drive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            } catch (Exception) {
                MessageBox.Show("Please select a valid WZ Image File.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (img.WzProperties.Count() == 0) {
                img.Dispose();
                fStream.Dispose();
                MessageBox.Show("This image file contained no data when parsing. Please select a different Maple Version.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var open = new FolderBrowserDialog();
            if (open.ShowDialog() != DialogResult.OK) {
                img.Dispose();
                open.Dispose();
                return;
            }
            var extractFolder = Path.Combine(open.SelectedPath, fName);
            if (File.Exists(extractFolder)) {
                extractFolder = GetValidFolderName(extractFolder, true);
            }
            if (Directory.Exists(extractFolder)) {
                var result = MessageBox.Show(extractFolder + " already exists.\r\nDo you want to overwrite that folder?\r\nNote: Clicking No will make a new folder.", "Folder Already Exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Cancel)
                    return;
                if (result != DialogResult.Yes) {
                    extractFolder = GetValidFolderName(Path.Combine(open.SelectedPath, fName), false);
                }
            }

            if (!Directory.Exists(extractFolder))
                Directory.CreateDirectory(extractFolder);
            DisableButtons();
            Info.AppendText("Dumping data from " + fName + " to " + extractFolder + "...\r\n");
            var startTime = DateTime.Now;
            CancelSource = new CancellationTokenSource();
            string startingPath = new DirectoryInfo(extractFolder).Name;
            new WzXml(this, open.SelectedPath, startingPath, includePngMp3Box.Checked, SelectedLinkType).DumpImage(img, startingPath);
            open.Dispose();
            img.Dispose();
            fStream.Dispose();
            CancelSource.Dispose();
            var duration = DateTime.Now - startTime;
            UpdateTextBoxInfo(Info, "完成提取 " + fName + " 成功 " + GetDurationAsString(duration), true);
            UpdateToolstripStatus("提取 " + fName + " successfully");
            EnableButtons();
        }

        private void CreateSingleDumperThread(WzFile file, WzXml wzxml, string fileName) {
            IsFinished = false;
            var startTime = DateTime.Now;
            var mainTask = Task.Factory.StartNew(() => DirectoryDumperThread(file, wzxml, true));
            mainTask.ContinueWith(p => {
                var duration = DateTime.Now - startTime;
                string message = String.Empty;
                if (CancelSource.Token.IsCancellationRequested) {
                    if (Exit)
                        return;
                    message = "取消提取 " + fileName;
                } else if (IsError) {
                    message = "提取中發生錯誤 " + fileName;
                } else {
                    message = "完成提取 " + fileName;
                }
                UpdateToolstripStatus(message);
                UpdateTextBoxInfo(Info, message + ".\r\n經過時間: " + GetDurationAsString(duration), true);
                IsError = false;
                IsFinished = true;
                EnableButtons();
                CancelSource.Dispose();
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void InitThread(string fileName, string dumpFolder, WzMapleVersion selectedValue) {
            WzListFile listFile = null;
            WzFile regFile = null;
            var message = String.Empty;
            try {
                if (fileName.EndsWith("List.wz", StringComparison.CurrentCultureIgnoreCase)) {
                    listFile = new WzListFile(fileName, selectedValue);
                    listFile.ParseWzFile();
                } else {
                    List<WzFile> s = new List<WzFile>();
                    getWzExtensionFiles(fileName, s);
                    regFile = new WzFile(fileName, selectedValue, s);
                    regFile.ParseWzFile();
                }
            } catch (IOException ex) {
                if (regFile != null) regFile.Dispose();
                message = "發生IO錯誤: " + ex.Message;
            } catch (UnauthorizedAccessException) {
                if (regFile != null) regFile.Dispose();
                message = "請以管理員權限重啟此程式";
            } catch (Exception ex) {
                if (regFile != null) regFile.Dispose();
                message = "處理檔案中發生問題: " + ex.Message;
            }
            if (!String.IsNullOrEmpty(message)) {
                UpdateTextBoxInfo(Info, "處理文件時發生錯誤 " + Path.GetFileName(fileName) + "\r\n訊息: " + message + "\r\nContinuing...", true);
                if (!fileName.EndsWith("List.wz"))
                    IsError = true;
                return;
            }
            if (regFile == null && listFile == null)
                return;
            var wzName = Path.GetFileName(fileName);
            var nFolder = Path.Combine(dumpFolder, wzName);
            if (listFile == null && includeVersionInFolderBox.Checked)
                nFolder += "_v" + regFile.Version;
            nFolder = GetValidFolderName(nFolder, false);
            if (!Directory.Exists(nFolder))
                Directory.CreateDirectory(nFolder);
            if (listFile == null)
                UpdateTextBoxInfo(versionBox, regFile.Version.ToString(CultureInfo.CurrentCulture), false);
            if (listFile != null) {
                UpdateTextBoxInfo(Info, "提取資料從 " + wzName + " 到 " + nFolder + "...", true);
            } else if (includePngMp3Box.Checked) {
                UpdateTextBoxInfo(Info, "提取 MP3, PNG 和 XML 從 " + wzName + " 到 " + nFolder + "...", true);
            } else {
                UpdateTextBoxInfo(Info, "提取 XML 從 " + wzName + " 到 " + nFolder + "...", true);
            }
            if (listFile != null) {
                DumpListWz(listFile, wzName, nFolder, DateTime.Now);
                listFile.Dispose();
            } else {
                DirectoryDumperThread(regFile, new WzXml(this, dumpFolder, new DirectoryInfo(nFolder).Name, includePngMp3Box.Checked, SelectedLinkType));
            }
        }

        private void CreateMultipleDumperThreads(string wzFolder, IEnumerable<string> files, string dumpFolder) {
            IsFinished = false;
            var startTime = DateTime.Now;
            CancelSource = new CancellationTokenSource();
            var t = Task.Factory.StartNew(() => {
                var pOps = new ParallelOptions { MaxDegreeOfParallelism = multiThreadCheckBox.Checked ? Math.Min(((string[])files).Length, (int)extractorThreadsNum.Value) : 1 };
                Parallel.ForEach(files, pOps, file => {
                    if (CancelSource.Token.IsCancellationRequested)
                        return;
                    InitThread(file, dumpFolder, SelectedVersion);
                });
            });
            t.ContinueWith(p => {
                var duration = DateTime.Now - startTime;
                if (CancelSource.Token.IsCancellationRequested) {
                    if (Exit)
                        return;
                    UpdateTextBoxInfo(Info, "取消提取WZ檔案. 花費時間: " + GetDurationAsString(duration), true);
                    UpdateToolstripStatus("提取WZ檔案取消");
                } else if (IsError) {
                    UpdateTextBoxInfo(Info, "提取WZ檔案中發生問題. 花費時間: " + GetDurationAsString(duration), true);
                    UpdateToolstripStatus("提取WZ檔案中發生問題");
                } else {
                    UpdateTextBoxInfo(Info, "提取所有WZ檔案從 " + wzFolder + " 到 " + GetDurationAsString(duration), true);
                    UpdateToolstripStatus("提取所有WZ 檔案成功");
                }
                IsError = false;
                IsFinished = true;
                EnableButtons();
                CancelSource.Dispose();
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void DirectoryDumperThread(WzDirectory dir, WzXml wzxml, bool singleDump = false) {
            if (CancelSource.Token.IsCancellationRequested)
                return;
            try {
                wzxml.DumpDir(dir);
                if (!singleDump && !CancelSource.Token.IsCancellationRequested)
                    UpdateTextBoxInfo(Info, "成功提取 " + dir.Name, true);
            } catch (Exception ex) {
                if (!CancelSource.Token.IsCancellationRequested) {
                    UpdateTextBoxInfo(Info, dir.Name + " 例外狀況: " + ex.Message + " " + ex.StackTrace, true);
                    IsError = true;
                }
            } finally {
                dir.Dispose();
            }
        }

        private static string GetDurationAsString(TimeSpan duration) {
            string s = String.Empty;
            if (duration.Hours != 0) {
                s += duration.Hours + " 小時";
              /*  if (duration.Hours != 1)
                    s += "s";*/
            }
            if (duration.Minutes != 0) {
                if (!string.IsNullOrEmpty(s))
                    s += ", ";
                s += duration.Minutes + " 分鐘";
               /* if (duration.Minutes != 1)
                    s += "s";*/
                s += ", ";
            }
            s += duration.Seconds + " 秒鐘";
            /*   if (duration.Seconds != 1)
                   s += "s";
               s += " and ";*/
            s += " ";
            s += duration.Milliseconds + " 毫秒";
         /*   if (duration.Milliseconds != 1)
                s += "s";*/
            return s;
        }

        private void CancelOperation(object sender, EventArgs e) {
            if (MessageBox.Show("請問是否要取消當前操作?", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            if (CancelSource != null)
                CancelSource.Cancel(true);
            CancelOpButton.Enabled = false;
            UpdateTextBoxInfo(Info, "取笑中... Waiting for the current image(s) to finish dumping...", true);
        }

        private void Form1Load(object sender, EventArgs e) {
            if (!File.Exists(Application.StartupPath + @"\MapleLib.dll")) {
                MessageBox.Show("請確認 MapleLib.dll 是否在同個目錄下.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
                return;
            }
            var args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i++) {
                string arg = args[i];
                if (arg.Equals("-a")) {
                    includePngMp3Box.Checked = true;
                } else if (arg.Equals("-o")) {
                    if (i + 1 < args.Length) {
                        string output = args[++i];
                        if (Directory.Exists(output))
                            outputFolderTB.Text = output;
                    }
                } else {
                    if (!arg.Equals("") && (arg.Contains("wz") || Directory.Exists(arg))) {
                        InputSelected(arg);
                    }
                }
            }
            MapleVersionComboBox.SelectedIndex = 0;
            LinkTypeComboBox.DataSource = Enum.GetValues(typeof(LinkType));
            LinkTypeComboBox.SelectedItem = IsElevated ? LinkType.Symbolic : LinkType.Hard;
        }

        private void Form1FormClosing(object sender, FormClosingEventArgs e) {
            if (IsFinished)
                return;
            if (MessageBox.Show("提取中 無法取消本程式. 是否要取消此操作?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                Exit = true;
                if (CancelSource != null)
                    CancelSource.Cancel(true);
            } else {
                e.Cancel = true;
            }
        }

        private void ClearInfoToolStripMenuItemClick(object sender, EventArgs e) {
            Info.Text = String.Empty;
            toolStripStatusLabel1.Text = String.Empty;
        }

        private void OpenFolder(object sender, EventArgs e) {
            Process.Start("explorer.exe", outputFolderTB.Text);
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e) {
            Close();
        }

        private static Form IsFormAlreadyOpen(Type formType) {
            return Application.OpenForms.Cast<Form>().FirstOrDefault(openForm => openForm.GetType() == formType);
        }

        private void AboutToolStripMenuItem1Click(object sender, EventArgs e) {
            Form taskFormName;
            if ((taskFormName = IsFormAlreadyOpen(typeof(About))) == null) {
                taskFormName = new About();
                taskFormName.Show();
            } else {
                taskFormName.WindowState = FormWindowState.Normal;
                taskFormName.BringToFront();
            }
        }

        private void EnableButtons() {
            SelectWzFileButton.Enabled = true;
            SelectWzFolder.Enabled = true;
            SelectExtractDestination.Enabled = true;
            LinkTypeComboBox.Enabled = includePngMp3Box.Checked;
            DumpWzButton.Enabled = true;
            CancelOpButton.Enabled = false;
            includePngMp3Box.Enabled = true;
            includeVersionInFolderBox.Enabled = true;
            MapleVersionComboBox.Enabled = true;
            multiThreadCheckBox.Enabled = true;
            if (!string.IsNullOrEmpty(outputFolderTB.Text))
                openFolderButton.Focus();
            else
                SelectWzFileButton.Focus();
            extractorThreadsLabel.Enabled = multiThreadCheckBox.Checked;
            extractorThreadsNum.Enabled = multiThreadCheckBox.Checked;
        }

        private void DisableButtons() {
            SelectWzFileButton.Enabled = false;
            SelectWzFolder.Enabled = false;
            SelectExtractDestination.Enabled = false;
            LinkTypeComboBox.Enabled = false;
            DumpWzButton.Enabled = false;
            CancelOpButton.Enabled = true;
            includePngMp3Box.Enabled = false;
            includeVersionInFolderBox.Enabled = false;
            MapleVersionComboBox.Enabled = false;
            multiThreadCheckBox.Enabled = false;
            extractorThreadsLabel.Enabled = false;
            extractorThreadsNum.Enabled = false;
            Info.Focus();
        }

        private static int Compare(string x, string y) {
            var file1 = new FileInfo(x);
            var file2 = new FileInfo(y);
            return Convert.ToInt32(file1.Length - file2.Length);
        }

        public void UpdateTextBoxInfo(TextBox tb, string info, bool appendNewLine) {
            if (appendNewLine)
                info += "\r\n";
            if (tb.InvokeRequired && tb.IsHandleCreated) {
                Invoke(new UpdateTextBoxDelegate(UpdateTextBox), new object[] { tb, info, appendNewLine });
            } else {
                UpdateTextBox(tb, info, appendNewLine);
            }
        }

        private void MapleVersionComboBoxKeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = true;
        }

        #region Nested type: UpdateTextBoxDelegate

        private delegate void UpdateTextBoxDelegate(TextBox tb, string info, bool append);

        #endregion

        private void MultiThreadCheckBox_CheckedChanged(object sender, EventArgs e) {
            extractorThreadsLabel.Enabled = multiThreadCheckBox.Checked;
            extractorThreadsNum.Enabled = multiThreadCheckBox.Checked;
        }

        private void CheckOutputPath() {
            if (outputFolderTB.Text.Length != 0) {
                if (includePngMp3Box.Checked && !LinkTypeComboBox.SelectedItem.Equals(LinkType.Copy)) {
                    String testFile = Path.Combine(outputFolderTB.Text, "test", "file");
                    String testFile2 = Path.Combine(outputFolderTB.Text, "test", "link");
                    FileInfo fi = new FileInfo(testFile);
                    fi.Directory.Create();
                    fi.Create().Close();
                    bool res = LinkTypeComboBox.SelectedItem.Equals(LinkType.Symbolic) ? CreateSymbolicLink(testFile2, testFile, 0) : CreateHardLink(testFile2, testFile, IntPtr.Zero);
                    if (!res) {
                        MessageBox.Show("A test link could not be created on the output drive. The Link Type will be changed to Copy.", "Unable to Create Test Link", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LinkTypeComboBox.SelectedItem = LinkType.Copy;
                    }
                    fi.Directory.Delete(true);
                }
            }
        }

        private void IncludePngMp3Box_CheckedChanged(object sender, EventArgs e) {
            LinkTypeComboBox.Enabled = includePngMp3Box.Checked;
            CheckOutputPath();
        }

        private void SelectWzFolder_Click(object sender, EventArgs e) {
            var open = new FolderBrowserDialog { Description = "請選擇你要提取Wz的文件夾" };
            if (open.ShowDialog() == DialogResult.OK) {
                var allFiles = Directory.GetFiles(open.SelectedPath, "*.wz");
                if (allFiles.Length == 0) {
                    MessageBox.Show("選擇的資料夾沒有Wz檔案. 請選擇其他資料夾.", "No WZ Files Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    InputSelected(open.SelectedPath);
                }
            }
            open.Dispose();
        }

        private void SelectExtractDestination_Click(object sender, EventArgs e) {
            var open = new FolderBrowserDialog { Description = "請選擇你要提取出來放置的文件夾" };
            if (open.ShowDialog() == DialogResult.OK) {
                outputFolderTB.Text = open.SelectedPath;
                DumpWzButton.Enabled = WZFileTB.Text.Length > 0 && outputFolderTB.Text.Length > 0;
                openFolderButton.Enabled = true;
                CheckOutputPath();
            }
            open.Dispose();

        }

        private void LinkTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (LinkTypeComboBox.SelectedItem.Equals(LinkType.Symbolic) && !IsElevated) {
                var result = MessageBox.Show("Creating symbolic links require administrative permission. Do you want to restart this program as an administrator?", "Admin Privileges Required", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes) {
                    try {
                        ProcessStartInfo processInfo = new ProcessStartInfo {
                            Verb = "runas",
                            FileName = Assembly.GetExecutingAssembly().Location,
                            UseShellExecute = true,
                            Arguments = "\"" + WZFileTB.Text + "\" -o \"" + outputFolderTB.Text + "\"" + (includePngMp3Box.Checked ? " -a" : "")
                        };
                        Process.Start(processInfo);
                        Close();
                    } catch (Exception) { }
                } else {
                    LinkTypeComboBox.SelectedItem = SelectedLinkType;
                }
            }
            SelectedLinkType = (LinkType)LinkTypeComboBox.SelectedItem;
        }

        private void LinkTypeComboBox_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = true;
        }
    }

    public enum LinkType : int {
        Hard = 1,
        Symbolic = 2,
        Copy = 3
    }
}