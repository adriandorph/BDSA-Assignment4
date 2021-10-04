using Assignment4.Core;
using System.Collections.Generic;
namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {
        public IEnumerable<TaskDTO> Tasks;
        public IReadOnlyCollection<TaskDTO> All(){
            return (IReadOnlyCollection<TaskDTO>)Tasks;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="task"></param>
        /// <returns>The id of the newly created task</returns>
        public int Create(TaskDTO task){
            return 0;
        }

        public void Delete(int taskId){
            
        }

        public TaskDetailsDTO FindById(int id){
            return null;
        }

        public void Update(TaskDTO task){
            
        }

        public void Dispose(){
            
        }
    }
}
