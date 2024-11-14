namespace RouteManagementTutorial.DTO
{
    public class CreateResult
    {
        public bool Success { get; set; } = false;
        public bool FirstName { get; set; } = false;
        public bool LastName { get; set; } = false;
        public bool EmailAvailable { get; set; } = false;
        public bool EmailValid { get; set; } = false!;
        public bool PasswordValid { get; set; } = false!;
        public bool PhoneNumberValid { get; set; } = false!;
        public bool LicenseType { get; set; } = false;
    }
}
