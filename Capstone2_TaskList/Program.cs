using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Capstone2_TaskList
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup empty list of UserTasks
            List<UserTask> mainTaskList = new List<UserTask>();

            //add a sample task 
            UserTask sampleTask = new UserTask("John", "Do the thing", "11/22/20", false);
            mainTaskList.Add(sampleTask);

            //main program begins
            ReturnToMainMenu(mainTaskList);
        }

        public static void ReturnToMainMenu(List<UserTask> userTaskList)
        {
            ListMainMenu();
            bool tryAgain = true;
            while (tryAgain)
            {
                string mainMenuInput = Console.ReadLine().ToLower();
                tryAgain = false;
                switch (mainMenuInput)
                {
                    case "1":
                    case "view current tasks":
                        ListCurrentTasks(userTaskList);
                        ReturnToMainMenu(userTaskList);
                        break;

                    case "2":
                    case "add a task":
                        AddTaskToList(userTaskList, "Let's add a task:");
                        break;

                    case "3":
                    case "remove a task":
                        RemoveTaskFromList(userTaskList, "Alright, which task would you like to remove?");
                        break;

                    case "4":
                    case "mark task complete":
                        MarkTaskComplete(userTaskList, "Which task would you like to mark complete?");
                        break;

                    case "5":
                    case "update task":
                        UpdateTaskFromList(userTaskList, "Which task would you like to update?");
                        break;

                    case "6":
                    case "exit":
                        ExitProgram();
                        break;

                    default:
                        tryAgain = true;
                        Console.WriteLine("Please make a valid selection");
                        break;
                }
            }
        }

        public static void ListCurrentTasks(List<UserTask> taskList)
        {
            Console.WriteLine("\nCurrent Tasks:");
            UserTask.ListTasks(taskList);
            Console.WriteLine("\nPress enter to return to main menu.\n");
            Console.ReadLine();
        }

        public static void AddTaskToList(List<UserTask> userTaskList, string message)
        {
            Console.WriteLine(message);

            //1) setup new task to add elements to
            UserTask newTask = new UserTask();

            //2) get input from user and add it to the new task
            Console.Write("Team Member: ");
            newTask.TeamMemberName = Console.ReadLine();

            Console.Write("Description: ");
            newTask.TaskDescription = Console.ReadLine();

            //2.5) date validation. mm/dd/yy format.
            bool dateCheck = true;
            string datePattern = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.]\d\d$";
            while (dateCheck)
            {
                Console.Write("Date: ");
                string dateInput = Console.ReadLine();
                if (!Regex.IsMatch(dateInput, datePattern))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid date (mm/dd/yy).");
                    Console.ResetColor();
                }
                else
                {
                    newTask.DueDate = dateInput;
                    dateCheck = false;
                }
            }

            //Don't really need to have this, since the TaskComplete property will default to false, but it's a nice sanity check.
            newTask.TaskComplete = false;

            //3) add task with filled in info, to the taskList
            userTaskList.Add(newTask);

            //4) print "task added"
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Task Successfully Added.");
            Console.ResetColor();

            //5) "Would you like to add another task"?
            bool tryAgain = true;
            while (tryAgain)
            {
                string userSelection = GetUserInput("Would you like to add another task? (y/n)");
                tryAgain = false;

                switch (userSelection)
                {
                    case "y":
                        AddTaskToList(userTaskList, message);
                        break;

                    case "n":
                        ReturnToMainMenu(userTaskList);
                        break;

                    default:
                        tryAgain = true;
                        Console.WriteLine("Please make a valid selection: (y or n)");
                        break;
                }
            }
        }

        public static void RemoveTaskFromList(List<UserTask> userTaskList, string message)
        {
            Console.WriteLine($"\n{message}");
            UserTask.ListTasks(userTaskList); //sanity check

            bool tryAgain = true;
            while (tryAgain)
            {

                //prompt user and get input
                Console.Write("Task # to delete: ");
                string userCoice = Console.ReadLine();
                int userChoiceAsNum;
                bool userChoiceTry = int.TryParse(userCoice, out userChoiceAsNum);

                //validation to make sure user input is a number, and that number is within the range of the task list
                if (!userChoiceTry || userChoiceAsNum > userTaskList.Count || userChoiceAsNum <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid task number.\n");
                    Console.ResetColor();
                    break;
                }

                //confirms task deletion. If yes, delete list item and ask if they'd like to delete another, else return to main menu
                string confirmDelete = GetUserInput("Are you sure? (y/n): ");
                tryAgain = false;
                switch (confirmDelete)
                {
                    case "y":
                        userTaskList.RemoveAt(userChoiceAsNum - 1);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Task successfully deleted.");
                        Console.ResetColor();
                        break;

                    case "n":
                        ReturnToMainMenu(userTaskList);
                        break;

                    default:
                        tryAgain = true;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please make a valid selection: (y or n)");
                        Console.ResetColor();
                        break;
                }
            }

            // asks if user would like to delte another item, if so, rerun method, else return to main menu
            bool removeAgain = true;
            while (removeAgain)
            {
                string userSelection = GetUserInput("Would you like to remove another task? (y/n): ");
                removeAgain = false;

                switch (userSelection)
                {
                    case "y":
                        RemoveTaskFromList(userTaskList, message);
                        break;

                    case "n":
                        ReturnToMainMenu(userTaskList);
                        break;

                    default:
                        removeAgain = true;
                        Console.WriteLine("Please make a valid selection: ('y' or 'n')");
                        break;
                }
            }
        }

        public static void MarkTaskComplete(List<UserTask> userTaskList, string message)
        {
            UserTask.ListTasks(userTaskList); //print current task list
            bool tryAgain = true;
            while (tryAgain)
            {

                //prompt user and get input
                Console.Write(message);
                string userCoice = Console.ReadLine();
                int userChoiceAsNum;
                bool userChoiceTry = int.TryParse(userCoice, out userChoiceAsNum);

                //validation to make sure user input is a number, and that number is within the range of the task list
                if (!userChoiceTry || userChoiceAsNum > userTaskList.Count || userChoiceAsNum <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid task number.\n");
                    Console.ResetColor();
                    break;
                }

                //confirms task deletion. If yes, delete list item and ask if they'd like to delete another, else return to main menu
                string confirmComplete = GetUserInput("Are you sure? (y/n): ");
                tryAgain = false;
                switch (confirmComplete)
                {
                    case "y":
                        //userTaskList.RemoveAt(userChoiceAsNum - 1);
                        userTaskList[userChoiceAsNum - 1].TaskComplete = true;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Task successfully completed.");
                        Console.ResetColor();
                        break;

                    case "n":
                        ReturnToMainMenu(userTaskList);
                        break;

                    default:
                        tryAgain = true;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please make a valid selection: (y or n)");
                        Console.ResetColor();
                        break;
                }
            }

            // asks if user would like to delte another item, if so, rerun method, else return to main menu
            bool removeAgain = true;
            while (removeAgain)
            {
                string userSelection = GetUserInput("Would you like to complete another task? (y/n): ");
                removeAgain = false;

                switch (userSelection)
                {
                    case "y":
                        MarkTaskComplete(userTaskList, message);
                        break;

                    case "n":
                        ReturnToMainMenu(userTaskList);
                        break;

                    default:
                        removeAgain = true;
                        Console.WriteLine("Please make a valid selection: ('y' or 'n')");
                        break;
                }
            }
        }

        public static void UpdateTaskFromList(List<UserTask> userTaskList, string message)
        {
            //which task would you like to update?
            {
                UserTask.ListTasks(userTaskList); //print current task list
                bool tryAgain = true;
                while (tryAgain)
                {
                    //prompt user and get input
                    Console.Write(message);
                    string userCoice = Console.ReadLine();
                    int userChoiceAsNum;
                    bool userChoiceTry = int.TryParse(userCoice, out userChoiceAsNum);

                    //validation to make sure user input is a number, and that number is within the range of the task list
                    if (!userChoiceTry || userChoiceAsNum > userTaskList.Count || userChoiceAsNum <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a valid task number.\n");
                        Console.ResetColor();
                        break;
                    }

                    string confirmUpdated = GetUserInput("Are you sure? (y/n): ");
                    tryAgain = false;
                    switch (confirmUpdated)
                    {
                        case "y":
                            //REAL UPDATES GO HERE - SEE ADD TASK METHOD
                            Console.Write("Team Member: ");
                            userTaskList[userChoiceAsNum - 1].TeamMemberName = Console.ReadLine();

                            //2) get input from user and add it to the new task
                            Console.Write("Description: ");
                            userTaskList[userChoiceAsNum - 1].TaskDescription = Console.ReadLine();


                            //2.5) date validation. mm/dd/yy format.
                            bool dateCheck = true;
                            string datePattern = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.]\d\d$";
                            while (dateCheck)
                            {
                                Console.Write("Date: ");
                                string dateInput = Console.ReadLine();
                                if (!Regex.IsMatch(dateInput, datePattern))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Please enter a valid date (mm/dd/yy).");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    userTaskList[userChoiceAsNum - 1].DueDate = dateInput;
                                    dateCheck = false;
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Task successfully updated.");
                            Console.ResetColor();
                            break;

                        case "n":
                            ReturnToMainMenu(userTaskList);
                            break;

                        default:
                            tryAgain = true;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please make a valid selection: (y or n)");
                            Console.ResetColor();
                            break;
                    }
                }
                // UserTask.ListTasks(userTaskList); //sanity check

                // asks if user would like to update another item, if so, rerun method, else return to main menu
                bool updateAgain = true;
                while (updateAgain)
                {
                    string userSelection = GetUserInput("Would you like to update another task? (y/n): ");
                    updateAgain = false;

                    switch (userSelection)
                    {
                        case "y":
                            UpdateTaskFromList(userTaskList, message);
                            break;

                        case "n":
                            ReturnToMainMenu(userTaskList);
                            break;

                        default:
                            updateAgain = true;
                            Console.WriteLine("Please make a valid selection: ('y' or 'n')");
                            break;
                    }
                }
            }
        }

        public static void ExitProgram()
        {
            Environment.Exit(0);
        }

        public static string GetUserInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        public static void ListMainMenu()
        {
            Console.WriteLine("\n\t\t\t\t====================================");
            Console.WriteLine("\t\t\t\tWelcome to Kyle's Task List Program");
            Console.WriteLine("\t\t\t\t====================================\n");

            Console.WriteLine("1. View Current Tasks   2. Add a Task   3. Remove a Task   4. Mark a Task Complete   5. Update Task   6. Exit");
        }
    }
}
