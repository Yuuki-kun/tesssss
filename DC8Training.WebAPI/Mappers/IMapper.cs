namespace DC8Training.WebAPI.Mappers
{
    public interface IMapper<A, B>
    {
        A MapFrom(B b);
        B MapTo(A a);
    }
}
