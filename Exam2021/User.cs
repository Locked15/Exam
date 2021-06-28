using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bool = System.Boolean;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace Exam2021
{
    public class User
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public String Name { get; set; }
        public String Password { get; set; }
        public List<Employer> Employeers { get; set; }
        public static List<User> AllUsers { get; private set; } = new List<User>(1);

        public User(String name, String password, List<Employer> employeers)
        {
            Name = name;
            Password = password;
            Employeers = employeers;
        }

        static User()
        {
            String path = Environment.CurrentDirectory;

            for (int i = 0; i < 2; i++)
            {
                path = path.Remove(path.LastIndexOf('\\'));
            }

            if (File.Exists(path + "\\Accounts.json"))
            {
                using (StreamReader sr1 = new StreamReader(path + "\\Accounts.json", System.Text.Encoding.Default))
                {
                    AllUsers = JsonSerializer.Deserialize<List<User>>(sr1.ReadToEnd(), options);
                }
            }
        }

        public static User CreateNewUser(String name, String password)
        {
            String path = Environment.CurrentDirectory;

            for (int i = 0; i < 2; i++)
            {
                path = path.Remove(path.LastIndexOf('\\'));
            }

            User newUser = new User(name, password, new List<Employer>(1));
            AllUsers.Add(newUser);

            using (StreamWriter sw1 = new StreamWriter(path + "\\Accounts.json", false, System.Text.Encoding.Default))
            {
                sw1.Write(JsonSerializer.Serialize<List<User>>(AllUsers, options));
            }

            return newUser;
        }

        public static Bool CheckName(String name)
        {
            foreach (User user in AllUsers)
            {
                if (user.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public Bool CheckUser()
        {
            foreach (User user in AllUsers)
            {
                if (user.Name == Name && user.Password == Password)
                {
                    return true;
                }
            }

            return false;
        }

        public async void CreateExcelFile()
        {
            await Task.Run(() =>
            {
                Excel.Application appToDo = new Excel.Application();
                appToDo.SheetsInNewWorkbook = Employeers.Count;

                Excel.Workbook mainWorkbook = appToDo.Workbooks.Add(1);
                Excel.Worksheet currentSheet = mainWorkbook.Worksheets.Item[1];

                for (int i = 0; i < Employeers.Count; i++)
                {
                    currentSheet.Name = Employeers[Employeers.Count - (i + 1)].Name;

                    currentSheet.Cells[1, 1] = "Номер сотрудника:";
                    currentSheet.Cells[2, 1] = "Имя сотрудника:";
                    currentSheet.Cells[3, 1] = "Дата рождения:";
                    currentSheet.Cells[4, 1] = "Должность:";

                    currentSheet.Cells[1, 2] = Employeers[i].Number;
                    currentSheet.Cells[2, 2] = Employeers[i].Name;
                    currentSheet.Cells[3, 2] = Employeers[i].BirthTime.ToString("dd.MM.yyyy!");
                    currentSheet.Cells[4, 2] = Employeers[i].Work;

                    currentSheet.Range["A1", "B4"].Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                    currentSheet.Range["A1", "B4"].Borders.Color = Excel.XlRgbColor.rgbBlack;
                    currentSheet.UsedRange.Columns.AutoFit();
                    currentSheet.UsedRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    if ((i + 1) < Employeers.Count)
                    {
                        currentSheet = mainWorkbook.Worksheets.Add();
                    }
                }

                mainWorkbook.Protect("Exam.");

                appToDo.Visible = true;
            });
        }

        public async void CreateExcelFile(String path, Bool openAfterCreating = false)
        {
            await Task.Run(() =>
            {
                Excel.Application appToDo = new Excel.Application();
                appToDo.SheetsInNewWorkbook = Employeers.Count;

                Excel.Workbook mainWorkbook = appToDo.Workbooks.Add(1);
                Excel.Worksheet currentSheet = mainWorkbook.Worksheets.Item[1];

                for (int i = 0; i < Employeers.Count; i++)
                {
                    currentSheet.Name = Employeers[Employeers.Count - (i + 1)].Name;

                    currentSheet.Cells[1, 1] = "Номер сотрудника:";
                    currentSheet.Cells[2, 1] = "Имя сотрудника:";
                    currentSheet.Cells[3, 1] = "Дата рождения:";
                    currentSheet.Cells[4, 1] = "Должность:";

                    currentSheet.Cells[1, 2] = Employeers[i].Number;
                    currentSheet.Cells[2, 2] = Employeers[i].Name;
                    currentSheet.Cells[3, 2] = Employeers[i].BirthTime.ToString("dd.MM.yyyy!");
                    currentSheet.Cells[4, 2] = Employeers[i].Work;

                    currentSheet.UsedRange.Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                    currentSheet.UsedRange.Borders.Color = Excel.XlRgbColor.rgbBlack;
                    currentSheet.UsedRange.Font.Name = "Georgia";
                    currentSheet.UsedRange.AutoFit();

                    if (i + 1 < Employeers.Count)
                    {
                        currentSheet = mainWorkbook.Worksheets.Add();
                    }
                }

                mainWorkbook.Protect("Exam.");

                if (Directory.Exists(path) && !File.Exists(path))
                {
                    if (!path.EndsWith(".xlsx"))
                    {
                        path += ".xlsx";
                    }

                    mainWorkbook.SaveAs(path);
                }

                if (openAfterCreating)
                {
                    appToDo.Visible = true;
                }

                else
                {
                    mainWorkbook.Close();
                    appToDo.Workbooks.Close();
                    appToDo.Quit();
                }
            });
        }

        public async void CreateWordFile()
        {
            await Task.Run(() =>
            {
                Word.Application appToDo = new Word.Application();

                appToDo.Documents.Add();
                Word.Document mainDocument = appToDo.Documents[1];

                for (int i = 0; i < Employeers.Count; i++)
                {
                    Word.HeaderFooter footer = mainDocument.Sections[i + 1].Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                    footer.LinkToPrevious = false;
                    footer.Range.Font.Name = "Georgia";
                    footer.Range.Font.Bold = 0;
                    footer.Range.Text = (i + 1).ToString();
                    footer.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    Word.Paragraph paragraphNumber = mainDocument.Content.Paragraphs.Add();
                    paragraphNumber.Range.InsertParagraphBefore();
                    paragraphNumber.Range.Text = "Номер сотрудника: " + Employeers[i].Number + ';';
                    paragraphNumber.Range.Font.Name = "Georgia";
                    paragraphNumber.Range.Font.Bold = 0;
                    paragraphNumber.Range.InsertParagraphBefore();

                    Word.Paragraph paragraphName = mainDocument.Content.Paragraphs.Add();
                    paragraphName.Range.InsertParagraphBefore();
                    paragraphName.Range.Text = "Имя сотрудника: " + Employeers[i].Name + ';';
                    paragraphName.Range.Font.Name = "Georgia";
                    paragraphName.Range.Font.Bold = 0;
                    paragraphName.Range.InsertParagraphBefore();

                    Word.Paragraph paragraphDate = mainDocument.Content.Paragraphs.Add();
                    paragraphDate.Range.InsertParagraphBefore();
                    paragraphDate.Range.Text = "Дата рождения: " + Employeers[i].BirthTime.ToString("dd.MM.yyyy!") + ';';
                    paragraphDate.Range.Font.Name = "Georgia";
                    paragraphDate.Range.Font.Bold = 0;
                    paragraphDate.Range.InsertParagraphBefore();

                    Word.Paragraph paragraphPlace = mainDocument.Content.Paragraphs.Add();
                    paragraphPlace.Range.InsertParagraphBefore();
                    paragraphPlace.Range.Text = "Должность: " + Employeers[i].Work + ';';
                    paragraphPlace.Range.Font.Name = "Georgia";
                    paragraphPlace.Range.Font.Bold = 0;

                    if ((i + 1) < Employeers.Count)
                    {
                        appToDo.ActiveDocument.Sections.Add();
                    }
                }

                mainDocument.Protect(Word.WdProtectionType.wdAllowOnlyReading,
                true, "Exam.");

                appToDo.Visible = true;
            });
        }

        public async void CreateWordFile(String path, Bool openAfterCreate)
        {
            await Task.Run(() =>
            {
                Word.Application appToDo = new Word.Application();
                Word.Document mainDocument = appToDo.Documents.Add(1);

                for (int i = 0; i < Employeers.Count; i++)
                {
                    Word.HeaderFooter footer = mainDocument.Sections[i + 1].Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                    footer.Range.Font.Name = "Georgia";
                    footer.Range.Font.Bold = 0;
                    footer.Range.Text = (i + 1).ToString();
                    footer.LinkToPrevious = false;

                    Word.Paragraph paragraph = mainDocument.Paragraphs.Add();
                    paragraph.Range.InsertParagraphBefore();
                    paragraph.Range.Text = "Номер сотрудника: " + Employeers[i].Number + ';';
                    paragraph.Range.Font.Name = "Georgia";
                    paragraph.Range.Font.Bold = 0;
                    paragraph.Range.InsertParagraphBefore();

                    paragraph = mainDocument.Paragraphs.Add();
                    paragraph.Range.InsertParagraphBefore();
                    paragraph.Range.Text = "Имя сотрудника: " + Employeers[i].Name + ';';
                    paragraph.Range.Font.Name = "Georgia";
                    paragraph.Range.Font.Bold = 0;
                    paragraph.Range.InsertParagraphBefore();

                    paragraph = mainDocument.Paragraphs.Add();
                    paragraph.Range.InsertParagraphBefore();
                    paragraph.Range.Text = "Дата рождения: " + Employeers[i].BirthTime.ToString("dd.MM.yyyy!") + ';';
                    paragraph.Range.Font.Name = "Georgia";
                    paragraph.Range.Font.Bold = 0;
                    paragraph.Range.InsertParagraphBefore();

                    paragraph = mainDocument.Paragraphs.Add();
                    paragraph.Range.InsertParagraphBefore();
                    paragraph.Range.Text = "Должность: " + Employeers[i].Work + ';';
                    paragraph.Range.Font.Name = "Georgia";
                    paragraph.Range.Font.Bold = 0;
                    paragraph.Range.InsertParagraphBefore();

                    if ((i + 1) < Employeers.Count)
                    {
                        mainDocument.Sections.Add();
                    }
                }

                mainDocument.Protect(Word.WdProtectionType.wdAllowOnlyReading,
                true, "Exam.");

                if (Directory.Exists(path) && !File.Exists(path))
                {
                    if (!path.EndsWith(".docx"))
                    {
                        path += ".docx";
                    }

                    mainDocument.SaveAs2(path);
                }

                if (openAfterCreate)
                {
                    appToDo.Visible = true;
                }

                else
                {
                    mainDocument.Close();
                    appToDo.Quit();
                }
            });
        }

        public static void SaveChanges()
        {
            String path = Environment.CurrentDirectory;

            for (int i = 0; i < 2; i++)
            {
                path = path.Remove(path.LastIndexOf('\\'));
            }

            using (StreamWriter sw1 = new StreamWriter(path + "\\Accounts.json", false, System.Text.Encoding.Default))
            {
                sw1.Write(JsonSerializer.Serialize<List<User>>(AllUsers, options));
            }
        }
    }
}
