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
                Console.WriteLine("\n1 - Создать заметку\n\n2 - Посмотреть заметки\n\n3 - Редактировать заметку\n\n4 - Найти заметку\n\n5 - Удалить заметку\n\n6 - Сохранить и выйти");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateNote(notes);
                        break;

                    case "2":
                        if (!notes.Any())
                        {
                            NotesIsEmpty();
                            break;
                        }

                        DisplayListNotes(notes);
                        modelNote = GetNoteOnID(notes);

                        if (modelNote == null)
                            break;
                        
                        DisplayContentNote(modelNote);
                        break;

                    case "3":
                        DisplayListNotes(notes);
                        modelNote = GetNoteOnID(notes);
                        if (modelNote == null)
                            break;
                        EditNote(modelNote);
                        break;

                    case "4":
                        SearchNote(notes);
                        break;

                    case "5":
                        DisplayListNotes(notes);
                        modelNote = GetNoteOnID(notes);
                        if (modelNote == null)
                            break;
                        DeleteNote(notes, modelNote);
                        break;

                    case "6":
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
            string title = GetNoteTitleOrContent("\nВернуться в меню - x\nВведите заголовок заметки: ");
            if (title == null)
                return;

            string content = GetNoteTitleOrContent("\nВернуться в меню - x\nВаша заметка ...");
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
                Console.WriteLine(message);
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

        static void DisplayListNotes(List<ModelNote> notes) 
        {
            Console.WriteLine("\n");
            Console.WriteLine($"{"ID", -5} {"Заголовок", -30} {"Дата", -30}");
            Console.WriteLine(new string('-', 70));
            foreach (var note in notes)
            {
                Console.WriteLine($"\n{note.Id, -5} {note.Title, -30} {note.Date.ToString("dd.MM.yyyy"), -30}");
            }
        }
        static void DisplayContentNote(ModelNote note)
        {
            Console.WriteLine(new string('-', 70) + "\n");
            Console.WriteLine($"Заголовок \n{note.Title}\n");
            Console.WriteLine($"Содержимое \n{note.Content}");
            Console.WriteLine("\n" + new string('-', 70) + "\n");
        }
        static ModelNote GetNoteOnID(List<ModelNote> notes) 
        {
            while (true)
            {
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

        static void EditNote(ModelNote note)
        {
            DisplayContentNote(note);
            Console.WriteLine("\n1 - Редактировать заголовок\n2 - Редактировать содержимое\n3 - Редактировать все\n");
            string input = Console.ReadLine();

            if (input == "1")
            {
                note.Title = GetNoteTitleOrContent("\nНе редактировать - x\nРедактируйте заголовок заметки: ") ?? note.Title;
                note.Date = DateTime.Now;
            }
            else if (input == "2")
            {
                note.Content = GetNoteTitleOrContent("\nНе редактировать = x\nРедактируйте содержимое заметки: ") ?? note.Content;
                note.Date = DateTime.Now;
            }
            else if (input == "3")
            {
                note.Title = GetNoteTitleOrContent("\nНе редактировать - x\nРедактируйте заголовок заметки: ") ?? note.Title;
                note.Content = GetNoteTitleOrContent("\nНе редактировать = x\nРедактируйте содержимое заметки: ") ?? note.Content;
                note.Date = DateTime.Now;
            }
            else
            {
                Console.WriteLine("\nНекоректный ввод!");
            }


        }

        static void DeleteNote(List<ModelNote> notes, ModelNote note)
        {
            DisplayContentNote(note);
            Console.WriteLine("\nОтменить удаление - x\n1 - Удалить заметку");
            string input = Console.ReadLine();

            if (input.ToLower() == "x")
                return;
            else if (input == "1")
                notes.Remove(note);
            else
                Console.WriteLine("\nНекоректный ввод!");
        }

        static void SearchNote(List<ModelNote> notes)
        {
            Console.WriteLine("\nВведите название...\n");
            string input = Console.ReadLine();

            List<ModelNote> foundNotes = new List<ModelNote>();
            foundNotes = notes.Where(n => n.Title.Contains(input, StringComparison.OrdinalIgnoreCase)).ToList();

            if (foundNotes.Any())
            {
                FoundNotes(foundNotes);

            }
            else
            {
                Console.WriteLine("Заметки не найдены");
            }
        }

        static void FoundNotes(List<ModelNote> foundNotes)
        {
            DisplayListNotes(foundNotes);
            Console.Write("Выбирете заметку: ");
            ModelNote note = GetNoteOnID(foundNotes);

            Console.WriteLine("\n1 - Просмотреть заметку\n2 - Редактировать заметку\n3 - Удалить заметку\n");
            string input = Console.ReadLine();

            if (input == "1")
            {
                DisplayContentNote(note);
            }
            else if (input == "2")
            {
                EditNote(note);
            }
            else if (input == "3")
            {
                DeleteNote(foundNotes, note);
            }
            else
            {
                Console.WriteLine("Некоректные данные");
            }
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