namespace WebRequests.Contracts.Base
{
    public class WebRequestDto<T> where T : WebResponseDto, new()
    {
        public T ConstructResponse() => new T();
    }
}