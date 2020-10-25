namespace TracerLibrary
{
    public interface ISerialize
    {
        byte[] Serialize(TraceResult traceResult);
    }
}
