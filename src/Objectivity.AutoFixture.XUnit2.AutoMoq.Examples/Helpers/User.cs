namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.Helpers
{
    public class User : IUser
    {
        public string Name { get; set; }

        public virtual User Substitute { get; set; }

        public string Surname { get; set; }
    }
}