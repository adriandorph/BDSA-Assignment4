using Assignment4.Core;
using System.Collections.Generic;
namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        public List<TaskDTO> Tasks;
        public IReadOnlyCollection<TaskDTO> All(){
            return (IReadOnlyCollection<TaskDTO>)Tasks;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="task"></param>
        /// <returns>The id of the newly created task</returns>
        public int Create(TaskDTO task){
            Tasks.Add(task);
            return task.Id;
        }

        public void Delete(int taskId){
            for(int i = 0; i<Tasks.Count; i++)
            {
                if(Tasks[i].Id == taskId){
                    Tasks.RemoveAt(i);
                    break;
                }
            }
        }

        public TaskDetailsDTO FindById(int id){
            for(int i = 0; i<Tasks.Count; i++)
            {
                if(Tasks[i].Id == id){
                    TaskDTO task = Tasks[i];
                    return null;
                }
            }
            return null;
        }

        public void Update(TaskDTO task){
            for(int i = 0; i<Tasks.Count; i++)
            {
                if(Tasks[i].Id == task.Id){
                    Tasks[i] = task;
                    break;
                }
            }
        }

        public void Dispose(){
            
        }
    }
}
