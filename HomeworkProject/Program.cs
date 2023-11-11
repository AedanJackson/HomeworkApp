using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApp
{
    internal class Program
    {
        class Homework
        {
            public readonly string Title;
            public readonly string Subject;
            public readonly DateTime DueDate;
            public readonly string Description;
            public bool Completed;

            public Homework(string title, DateTime dueDate, string subject, string description, bool completed = false)
            {
                Title = title;
                Subject = subject;
                DueDate = dueDate;
                Description = description;
                Completed = completed;
            }

            public void CompleteHomework()
            {
                Completed = true;
            }
        }

        class HomeworkList
        {
            public List<Homework> Tasks;

            public HomeworkList()
            {
                Tasks = new List<Homework>();
            }

            public void Add(Homework Task)
            {
                Tasks.Add(Task);
            }

            // Sorts (insertion) the homework items in the list by date, with the one due first at the front.
            public void SortByDate()
            {
                Tasks = Tasks.OrderBy(h => h.DueDate).ToList<Homework>();
            }
        }

        // class for handling files
        class FileHandler
        {
            private string FileName;

            public FileHandler(string FileName)
            {
                this.FileName = FileName;
            }

            // Reads the homework tasks from a file to a list, else creates the file if it does not already exist.
            public void ReadHomeworks(HomeworkList HomeworkList)
            {
                try
                {
                    StreamReader HomeworkFile = new StreamReader(FileName);
                    while (!HomeworkFile.EndOfStream)
                    {
                        string Title = HomeworkFile.ReadLine();
                        DateTime DueDate = Convert.ToDateTime(HomeworkFile.ReadLine());
                        string Subject = HomeworkFile.ReadLine();
                        string Description = HomeworkFile.ReadLine();
                        bool Completed = bool.Parse(HomeworkFile.ReadLine());
                        HomeworkList.Add(new Homework(Title, DueDate, Subject, Description, Completed));
                    }
                    HomeworkFile.Close();
                }
                catch
                {
                    StreamWriter HomeworkFile = new StreamWriter(FileName);
                    HomeworkFile.Close();
                }
            }

            // Writes the homework list to the file
            public void WriteHomeworks(List<Homework> HomeworkList)
            {
                // Write stuff here
            }
        }

        static void Main(string[] args)
        {
            const string FileName = "homework.txt";
            FileHandler File = new FileHandler(FileName);
            HomeworkList HomeworkList = new HomeworkList();
            File.ReadHomeworks(HomeworkList);
            PrintMenu(HomeworkList);
            Console.ReadLine();
        }

        // Prints the menu for the program.
        static void PrintMenu(HomeworkList HomeworkList)
        {
            // List to store homework tasks that are...
            // ...due in the next 3 days
            HomeworkList RedText = new HomeworkList();
            //... not completed
            HomeworkList OrangeText = new HomeworkList();
            //... completed
            HomeworkList GreenText = new HomeworkList();
            //try 
            //{
            foreach (Homework Task in HomeworkList.Tasks)
            {
                // Organises tasks into text colour (Due less than 3 days, uncompleted, and completed respectively)
                if (Task.Completed)
                {
                    GreenText.Add(Task);
                }
                else if (Task.DueDate < DateTime.Now + TimeSpan.FromDays(3))
                {
                    RedText.Add(Task);
                }
                else
                {
                    OrangeText.Add(Task);
                }

            }
            RedText.SortByDate();
            OrangeText.SortByDate();
            GreenText.SortByDate();
            //}
            //catch
            //{
            //    Console.WriteLine("No homework!");
            //}

            // Prints tasks due in less than 3 days and not completed
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (Homework Task in RedText.Tasks)
            {
                Console.WriteLine($"Assignment: {Task.Title}");
                Console.Write($"Due: {Task.DueDate.Day}/{Task.DueDate.Month}/{Task.DueDate.Year} ");
                // Highlights the amount of days left
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Red;
                if ((Task.DueDate - DateTime.Now).Days == 1)
                {
                    Console.WriteLine($"({(Task.DueDate - DateTime.Now).Days} DAY)");
                }
                else
                {
                    Console.WriteLine($"({(Task.DueDate - DateTime.Now).Days} DAYS)");
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($"Subject: {Task.Subject}");
                Console.WriteLine($"Description: {Task.Description}");
                Console.WriteLine();
            }

            // Prints tasks not completed
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Homework Task in OrangeText.Tasks)
            {
                Console.WriteLine($"Assignment: {Task.Title}");
                Console.Write($"Due: {Task.DueDate.Day}/{Task.DueDate.Month}/{Task.DueDate.Year} ");
                Console.WriteLine($"({(Task.DueDate - DateTime.Now).Days} days)");
                Console.WriteLine($"Subject: {Task.Subject}");
                Console.WriteLine($"Description: {Task.Description}");
                Console.WriteLine();
            }

            // Prints tasks that are completed
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Completed homework:");
            foreach (Homework Task in GreenText.Tasks)
            {
                Console.WriteLine($"Assignment: {Task.Title}");
                Console.Write($"Due: {Task.DueDate.Day}/{Task.DueDate.Month}/{Task.DueDate.Year} ");
                Console.WriteLine($"({(Task.DueDate - DateTime.Now).Days} days)");
                Console.WriteLine($"Subject: {Task.Subject}");
                Console.WriteLine($"Description: {Task.Description}");
                Console.WriteLine();
            }
        }
    }
}
