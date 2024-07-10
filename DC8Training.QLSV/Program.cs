using DC8Training.QLSV.Helpers;
using DC8Training.QLSV.Models;
using Newtonsoft.Json;

namespace DC8Training.QLSV
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var students = new List<Student>();
            var random = new Random();
            for (int i = 0; i < 500; i++)
            {
                var isMan = random.Next(2) == 0;
                students.Add(new Student { Id = i + 1, Name = MyHelper.GenerateName(isMan), Gender = isMan ? Enums.Gender.Man : Enums.Gender.Woman});
                //Console.WriteLine(students[i].Name + " " + students[i].Gender);
            }
            Console.WriteLine(JsonConvert.SerializeObject(students));
        }
    }
}
