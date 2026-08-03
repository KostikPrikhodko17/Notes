using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes
{
    class Program
    {
        static void Main()
        {
            ModelNote modelNote = new ModelNote();
            List<ModelNote> notes = new List<ModelNote>();

            Console.WriteLine("\n1 - Создать заметку");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    CreateNote(notes);
                    break;

                default:
                    Console.WriteLine("\nНекоректный ввод!");
                    break;
            }
        }

        static void CreateNote(List<ModelNote> notes)
        {
            string message = "Введите заголовок заметки: ";
            string title = GetNoteTitleOrContent(message);
            if (title == null)
                return;

            message = "Ваша заметка ...";
            string content = GetNoteTitleOrContent(message);
            if (content == null)
                return;

            notes.Add(new ModelNote
            {
                Id = notes.Count + 1,
                Title = title,
                Content = content,
                Date = DateTime.Now,
            });


        }
        static string GetNoteTitleOrContent(string message)
        {
            while (true)
            {
                Console.WriteLine($"\nВернуться в меню - x\n{message}\n");
                string input = Console.ReadLine();

                if (input.ToLower() != "x" && !string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                else if (input.ToLower() == "x")
                {
                    return null;
                }
                else
                {
                    Console.WriteLine("\nНекоректный ввод!");
                }
            }
        }
    }
}