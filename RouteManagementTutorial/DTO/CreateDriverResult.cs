namespace RouteManagementTutorial.DTO
{
    public class CreateDriverResult
    {
        public bool Success { get; set; } = false;
        public bool EmailAvailable { get; set; } = false;
        public bool EmailValid { get; set; } = false!;
        public bool PhoneNumberValid { get; set; } = false!;
    }
}
