using Microsoft.EntityFrameworkCore;
using ToDos.Models;

namespace ToDos.Services
{
    public class TodoService
    {
        private readonly AppDbContext _context;

        public TodoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TodosVM>> GetTodosAsync()
        {
            return await _context.Todos.OrderBy(t => t.Id).ToListAsync();
        }

        public async Task AddTodoAsync(TodosVM todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTodoAsync(TodosVM todo)
        {
            var existingTodo = await _context.Todos.FindAsync(todo.Id);

            if (existingTodo == null)
                return;

            existingTodo.Libelle = todo.Libelle;
            existingTodo.Commentaire = todo.Commentaire;
            existingTodo.Date_planif = todo.Date_planif;
            existingTodo.Date_realisation = todo.Date_realisation;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTodoAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
                return;

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }


    }
}
