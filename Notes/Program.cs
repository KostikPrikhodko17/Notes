using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
using System.Text.Unicode;
using System.Text.Encodings.Web;

namespace Notes
{
    class Program
    {
        const string FILE_PATH = "notes.txt";
        static void Main()
        {
            ModelNote modelNote = new ModelNote();
            List<ModelNote> notes = new List<ModelNote>();

            if (File.Exists(FILE_PATH))
            {
                string data = File.ReadAllText(FILE_PATH);
                notes = JsonSerializer.Deserialize<List<ModelNote>>(data) ?? new List<ModelNote>();
            }

            bool isRunning = true;

            do
            {
                Console.WriteLine("\n1 - Создать заметку\n\n2 - Посмотреть заметки\n\n3 - Сохранить и выйти");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateNote(notes);
                        Console.WriteLine("\n\n");
                        break;

                    case "2":
                        if (!notes.Any())
                        {
                            NotesIsEmpty();
                            break;
                        }
                        DisplayNoteContent(notes);
                        Console.WriteLine("\n\n");
                        break;

                    case "3":
                        SaveToData(notes);
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("\nНекоректный ввод!");
                        break;
                }
            }
            while (isRunning);


        }

        static void NotesIsEmpty()
        {
            Console.WriteLine("\nСписок заметок пуст!");
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
                Console.WriteLine($"\n\nВернуться в меню - x\n{message}\n");
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

        static void ShowListNotes(List<ModelNote> notes) 
        {
            Console.WriteLine("\n");
            Console.WriteLine($"{"ID", -5} {"Заголовок", -30} {"Дата", -30}");
            Console.WriteLine(new string('-', 70));
            foreach (var note in notes)
            {
                Console.WriteLine($"\n{note.Id, -5} {note.Title, -30} {note.Date.ToString("dd.MM.yyyy"), -30}");
            }
        }

        static ModelNote GetNote(List<ModelNote> notes)
        {
            while (true)
            {
                ShowListNotes(notes);

                Console.WriteLine("\n\nВернуться в меню - x\nВведите ID заметки\n");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int noteId))
                {
                    var note = notes.FirstOrDefault(n => n.Id == noteId);

                    if (note != null)
                    {
                        return note;
                    }
                    else
                    {
                        Console.WriteLine("\nЗаметка с таким ID не найдена!");
                    }
                }
                else if (input.ToLower() == "x")
                {
                    return null;
                }
                else
                {
                    Console.WriteLine("Некоректные данные");
                }
            }

        }

        static void DisplayNoteContent(List<ModelNote> notes)
        {
            ModelNote note = GetNote(notes);

            if (note == null)
                return;

            Console.WriteLine(new string('-', 70) + "\n");
            Console.WriteLine($"\t\t\t{note.Title}\n");
            Console.WriteLine(note.Content);
            Console.WriteLine("\n" + new string('-', 70) + "\n");
        }



        static void SaveToData(List<ModelNote> notes)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            try
            {
                string data = JsonSerializer.Serialize(notes, options);
                File.WriteAllText(FILE_PATH, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
            }
        }
    }
}