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

        public Task<List<TodosVM>> GetTodosAsync()
            => GetTodosAsync(new TodoFilter());

        public async Task<List<TodosVM>> GetTodosAsync(TodoFilter filter)
        {
            IQueryable<TodosVM> query = _context.Todos;

            // Recherche texte (Libellé + Commentaire)
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var s = filter.Search.Trim();
                query = query.Where(t =>
                    t.Libelle.Contains(s) ||
                    (t.Commentaire != null && t.Commentaire.Contains(s)));
            }

            // Statut (fait / non fait)
            if (filter.IsDone.HasValue)
            {
                if (filter.IsDone.Value)
                    query = query.Where(t => t.Date_realisation != null);
                else
                    query = query.Where(t => t.Date_realisation == null);
            }

            // Date planifiée
            if (filter.PlanifFrom.HasValue)
                query = query.Where(t => t.Date_planif >= filter.PlanifFrom.Value.Date);

            if (filter.PlanifTo.HasValue)
                query = query.Where(t => t.Date_planif <= filter.PlanifTo.Value.Date);

            // Date de réalisation
            if (filter.DoneFrom.HasValue)
                query = query.Where(t => t.Date_realisation >= filter.DoneFrom.Value.Date);

            if (filter.DoneTo.HasValue)
                query = query.Where(t => t.Date_realisation <= filter.DoneTo.Value.Date);

            return await query
                .OrderBy(t => t.Id)
                .ToListAsync();
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
