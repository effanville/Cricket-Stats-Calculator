namespace CricketStructures.Player.Interfaces
{
    public interface ICricketPlayer
    {
        /// <summary>
        /// The name by which this player is known.
        /// </summary>
        PlayerName Name
        {
            get;
        }

        /// <summary>
        /// Method to edit the name of the player.
        /// </summary>
        void EditName(string surname, string forename);
    }
}
