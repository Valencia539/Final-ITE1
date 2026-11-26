using System;
using System.Collections.Generic;

class Program
{
    static Phonebook phonebook = new Phonebook();
    static void Main()
    {
        int input = 0;
        while (input != 4)
        {
            Console.WriteLine("\n[1] Store to ASEAN phonebook\n[2] Edit entry in ASEAN phonebook\n[3] Search ASEAN phonebook by country\n[4] Exit");

            Console.Write("Enter choice: ");
            
            if (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Invalid input. Try again.");
                continue;
            }

            switch (input)
            {
                case 1:
                    StoreStudent();
                    break;

                case 2:
                    EditStudent();
                    break;

                case 3:
                    SearchStudents();
                    break;

                case 4:
                    Console.WriteLine("Exiting Program...");
                    break;

                default:
                    Console.WriteLine("Invalid input. Try again.");
                    break;
            }
        }
    }
    static void StoreStudent()
    {
        string again = "Y";
        do
        {
            Console.WriteLine("\n===== STORE STUDENT =====\nEnter student number: ");
            string studentNumber = Console.ReadLine();

            if (phonebook.StudentNumberExists(studentNumber))
            {
                Console.WriteLine("Student number already exists.");
                continue;
            }
            Console.Write("Enter surname: ");
            string surname = Console.ReadLine();
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter occupation: ");
            string occupation = Console.ReadLine();
            char gender;

            while (true)
            {
                Console.Write("Enter gender (M/F): ");
                string input = Console.ReadLine().ToUpper();
                if (input == "M" || input == "F")
                {
                    gender = input[0];
                    break;
                }
                Console.WriteLine("Invalid gender. Enter M or F.");
            }

            int countryCode;
            while (true)
            {
                Console.Write("Enter country code: ");
                if (int.TryParse(Console.ReadLine(), out countryCode) && phonebook. IsValidCountryCode(countryCode))
                {
                    break;
                }
                Console.WriteLine("Invalid country code. Try again.");
            }

            int areaCode;

            while (true)
            {
                Console.Write("Enter area code: ");
                if (int.TryParse(Console.ReadLine(), out areaCode) && areaCode >= 0)
                {
                    break;
                }
                Console.WriteLine("Invalid area code. Try again.");
            }
            Console.Write("Enter number: ");
            string phoneNumber = Console.ReadLine();
            Student student = new Student(studentNumber,surname, firstName, occupation, gender,countryCode, areaCode, phoneNumber);
            phonebook.AddStudent(student);
            Console.WriteLine("Student successfully added!\nDo you want to enter another entry [Y/N]? ");
            again = Console.ReadLine().ToUpper();
        } while (again == "Y");
    }

    static void EditStudent()
    {
        Console.WriteLine("\n===== EDIT STUDENT =====\nEnter student number: ");
        string studentNumber = Console.ReadLine();
        Student student = phonebook.FindStudent(studentNumber);
        if (student == null)
        {
            Console.WriteLine("Student number does not exist.");
            return;
        }

        int choice = 0;
        while (choice != 8)
        {
            Console.WriteLine($"\nHere is the existing information about {student.StudentNumber}:");
            phonebook.DisplayStudent(student);
            Console.WriteLine("\nWhich information do you wish to change?\n[1] Student number\n[2] Surname\n[3] Gender\n[4] Occupation\n[5] Country code\n[6] Area code\n[7] Phone number\n[8] None - Go back to main menu\nEnter choice: ");

            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid choice.");
                choice = 0;
                continue;
            }

            if (choice >= 1 && choice <= 7)
            {
                phonebook.EditStudent(student, choice);
                Console.WriteLine("Information updated!");
            }
            else if (choice != 8)
            {
                Console.WriteLine("Invalid choice.");
            }
        }
    }

    static void SearchStudents()
    {
        Console.WriteLine("\n===== SEARCH BY COUNTRY =====\nFrom which country:\n[1] Philippines\n[2] Thailand\n[3] Singapore\n[4] Indonesia\n[5] Malaysia\n[6] ALL\n[0] No More");

        List<int> selectedCodes = new List<int>();
        int[] countryCodes = {63, 66, 65, 62, 60};

        int choice;
        while (true)
        {
            Console.Write("Enter choice: ");

            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid choice.");
                continue;
            }

            if (choice == 0)
            {
                break;
            }

            if (choice == 6)
            {
                selectedCodes = phonebook.GetAllCountryCodes();
                break;
            }

            if (choice >= 1 && choice <= 5)
            {
                int code = countryCodes[choice - 1];

                if (!selectedCodes.Contains(code))
                {
                    selectedCodes.Add(code);
                    Console.WriteLine("Country added.");
                }
                else
                {
                    Console.WriteLine("Country already selected.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        if (selectedCodes.Count == 0)
        {
            Console.WriteLine("No countries selected.");
            return;
        }

        List<Student> results = phonebook.SearchByCountry(selectedCodes);

        Console.WriteLine($"\nHere are the students from {phonebook.GetSelectedCountries(selectedCodes)}:");
        phonebook.DisplayStudents(results);
    }
}

class Student
{
    public string StudentNumber { get; set; }
    public string Surname { get; set; }
    public string FirstName { get; set; }
    public string Occupation { get; set; }
    public char Gender { get; set; }
    public int CountryCode { get; set; }
    public int AreaCode { get; set; }
    public string PhoneNumber { get; set; }

    public Student(string studentNumber, string surname, string firstName, string occupation, char gender, int countryCode, int areaCode, string phoneNumber)
    {
        StudentNumber = studentNumber;
        Surname = surname;
        FirstName = firstName;
        Occupation = occupation;
        Gender = gender;
        CountryCode = countryCode;
        AreaCode = areaCode;
        PhoneNumber = phoneNumber;
    }
}

class Phonebook
{
    private List<Student> students = new List<Student>();

    private Dictionary<int, string> countries = new Dictionary<int, string>()
    {
        { 60, "Malaysia" },
        { 62, "Indonesia" },
        { 63, "Philippines" },
        { 65, "Singapore" },
        { 66, "Thailand" }
    };
    public bool IsValidCountryCode(int code)
    {
        return countries.ContainsKey(code);
    }
    public string GetCountryName(int code)
    {
        if (countries.ContainsKey(code))
            return countries[code];

        return "Unknown";
    }
    public Student FindStudent(string studentNumber)
    {
        for (int i = 0; i < students.Count; i++)
        {
            if (students[i].StudentNumber == studentNumber)
            {
                return students[i];
            }
        }
        return null;
    }
    public bool StudentNumberExists(string studentNumber)
    {
        return FindStudent(studentNumber) != null;
    }
    public bool AddStudent(Student student)
    {
        if (StudentNumberExists(student.StudentNumber)){return false;}
        students.Add(student);
        return true;
    }
    public void EditStudent(Student student, int choice)
    {
        switch (choice)
        {
            case 1:
                Console.Write("Enter new student number: ");
                string newNumber = Console.ReadLine();
                Student existing = FindStudent(newNumber);
                if (existing != null && existing != student)
                {
                    Console.WriteLine("Student number already exists.");
                    break;
                }
                student.StudentNumber = newNumber;
                break;
            case 2:
                Console.Write("Enter new surname: ");
                student.Surname = Console.ReadLine();
                break;
            case 3:
                Console.Write("Enter new gender (M/F): ");
                string input = Console.ReadLine().ToUpper();
                if (input == "M" || input == "F")
                {
                    student.Gender = input[0];
                }
                else
                {
                    Console.WriteLine("Invalid gender.");
                }
                break;
            case 4:
                Console.Write("Enter new occupation: ");
                student.Occupation = Console.ReadLine();
                break;
            case 5:
                Console.Write("Enter new country code: ");
                if (int.TryParse(Console.ReadLine(), out int code) && IsValidCountryCode(code))
                {
                    student.CountryCode = code;
                }
                else
                {
                    Console.WriteLine("Invalid country code.");
                }
                break;
            case 6:
                Console.Write("Enter new area code: ");
                if (int.TryParse(Console.ReadLine(), out int area) && area >= 0)
                {
                    student.AreaCode = area;
                }
                else
                {
                    Console.WriteLine("Invalid area code.");
                }
                break;
            case 7:
                Console.Write("Enter new phone number: ");
                student.PhoneNumber = Console.ReadLine();
                break;
        }
    }

   public List<Student> SearchByCountry(List<int> selectedCodes)
    {
        List<Student> results = new List<Student>();
        foreach (Student s in students)
        {
            if (selectedCodes.Contains(s.CountryCode))
            {
                results.Add(s);
            }
        }
        results.Sort((a, b) => string.Compare(a.Surname, b.Surname, true));
        return results;
    }
    public void DisplayStudent(Student student)
    {
        Console.WriteLine($"{student.FirstName} {student.Surname} is a {student.Occupation}. His/Her number is {student.CountryCode}-{student.AreaCode}-{student.PhoneNumber}");
    }
    public void DisplayStudents(List<Student> results)
    {
        if (results.Count == 0)
        {
            Console.WriteLine("No students found.");
            return;
        }
        foreach (Student student in results)
        {
            Console.WriteLine($"{student.Surname}, {student.FirstName}, with student number {student.StudentNumber}, is a {student.Occupation}. His/Her phone number is {student.CountryCode}-{student.AreaCode}-{student.PhoneNumber}");
        }
    }
    public string GetSelectedCountries(List<int> codes)
    {
        string result = "";
        for (int i = 0; i < codes.Count; i++)
        {
            if (i > 0)
            {
                result += " and ";
            }
            result += countries[codes[i]];
        }
        return result;
    }
    public List<int> GetAllCountryCodes()
    {
        return new List<int>(countries.Keys);
    }
}
