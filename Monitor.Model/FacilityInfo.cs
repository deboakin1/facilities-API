namespace Monitor.Model
{
    /// <summary>
    /// Models the facilities data
    /// </summary>
    /// <param name="Patient_Id"></param>
    /// <param name="Name"></param>
    /// <param name="Date_Created"></param>
    /// <param name="Submissions"></param>
    public class FacilityInfo
    {
        public DateTime DateCreated { get; init; }
        public string Name { get; init; }
        public long PatientId { get; init; }
        public int Submissions { get; init; }
    }

    public class UpdatedFacilty : FacilityInfo
    {
        public int Id { get; set; }
    }

}