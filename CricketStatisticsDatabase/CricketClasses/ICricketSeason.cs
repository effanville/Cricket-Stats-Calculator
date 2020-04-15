﻿using System;
using System.Collections.Generic;

namespace Cricket.Interfaces
{
    public interface ICricketSeason
    {
        /// <summary>
        /// The name associated to this season.
        /// </summary>
        string Name
        { 
            get;
        }

        /// <summary>
        /// The year this season was held in.
        /// </summary>
        DateTime Year
        { 
            get;
        }

        /// <summary>
        /// The players that played in this season.
        /// </summary>
        List<ICricketPlayer> Players
        { 
            get;
        }

        /// <summary>
        /// All matches played in this season.
        /// </summary>
        List<ICricketMatch> Matches
        { 
            get;
        }

        /// <summary>
        /// Queries whether the given data is the same as this seasons.
        /// </summary>
        bool SameSeason(DateTime year, string name);

        /// <summary>
        /// Queries whether the given object is the same as this season.
        /// </summary>
        bool Equals(object obj);

        /// <summary>
        /// Adds a match in the list with a given identifier.
        /// </summary>
        /// <remarks>
        /// What to do if it can return two?
        /// </remarks>
        bool AddMatch();

        /// <summary>
        /// Returns the match in the list with a given identifier.
        /// </summary>
        /// <remarks>
        /// What to do if it can return two?
        /// </remarks>
        ICricketMatch GetMatch();

        /// <summary>
        /// Returns whether the match with a given identifier exists.
        /// </summary>
        /// <remarks>
        /// What to do if it can return two?
        /// </remarks>
        bool ContainsMatch();

        /// <summary>
        /// Removes the match in the list with a given identifier.
        /// </summary>
        /// <remarks>
        /// What to do if it can return two?
        /// </remarks>
        bool RemoveMatch();
    }
}