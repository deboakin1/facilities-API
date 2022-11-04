namespace Monitor.Model
{
    /// <summary>
    /// Models the facilities data
    /// </summary>
    /// <param name="Patient_Id"></param>
    /// <param name="Name"></param>
    /// <param name="Date_Created"></param>
    /// <param name="Submissions"></param>
    public record FacilityInfo(long PatientId, string Name, DateTime DateCreated, int Submissions);
}