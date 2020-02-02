using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone2_TaskList
{
    class UserTask
    {
        //fields
        //1) Team member's name, 2) task description, 3) due date(DateTime) 4) isComplete(bool)
        private string teamMemberName;
        private string taskDescription;
        private string dueDate;
        private bool taskComplete;

        //properties
        public string TeamMemberName
        {
            get { return teamMemberName; }
            set { teamMemberName = value; }
        }
        public string TaskDescription
        {
            get { return taskDescription; }
            set { taskDescription = value; }
        }
        public string DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }
        public bool TaskComplete
        {
            get { return taskComplete; }
            set { taskComplete = value; }
        }

        //constructors
        public UserTask()
            {

            }

        public UserTask(string _teamMemberName, string _taskDescription, string _dueDate, bool _taskComplete)
        {
            teamMemberName = _teamMemberName;
            taskDescription = _taskDescription;
            dueDate = _dueDate;
            taskComplete = _taskComplete;
        }

        //methods
        public static void ListTasks(List<UserTask> taskList)
        {
            foreach (UserTask taskElement in taskList)
            {
                Console.WriteLine($"{taskList.IndexOf(taskElement) + 1} | Name: {taskElement.teamMemberName}\t\tDue Date:{taskElement.dueDate}\tComplete: {taskElement.taskComplete}\t\tDescription: {taskElement.taskDescription}");
            }
        }

    }
}
