using NMSBackend.Model;

namespace NMSBackend.Repositories
{
    public interface IUserRepository : IRepositoryBase<User> { }
    public class UserRepository : RepositoryBase<User>, IUserRepository { public UserRepository() : base() { } }
    public interface ITokenRepository : IRepositoryBase<Token> { }
    public class TokenRepository : RepositoryBase<Token>, ITokenRepository { public TokenRepository() : base() { } }
    public interface INoteRepository : IRepositoryBase<Note> { }
    public class NoteRepository : RepositoryBase<Note>, INoteRepository { public NoteRepository() : base() { } }
}
