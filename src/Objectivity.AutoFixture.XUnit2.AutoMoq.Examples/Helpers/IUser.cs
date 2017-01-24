namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.Helpers
{
    public interface IUser
    {
        string Name { get; set; }

        User Substitute { get; set; }

        string Surname { get; set; }
    }
}