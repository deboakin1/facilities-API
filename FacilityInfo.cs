using System;

namespace Facilities.Model
{
    public record FacilityInfo(long Patient_Id, string Name, DateTimeOffset Date_Created, bool Submissions);
}