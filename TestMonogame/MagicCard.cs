using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TestMonogame;



namespace MTGGame
{
    #region Enums 
    enum TapState
    {
        Untapped,
        Tapped,
    }
    #endregion

    #region Gamethings

    #region GameScreen

    #endregion

    #region TheStack

    public class Stack
    {
        public List<IMagicEffect> Thing;
        private Stack()
        {

        }

        public static Stack()
        {
            
        }
    }

    #endregion

    #endregion

    #region Magic Card Class

    class Card
    {
    }

    #endregion



    #region Interfaces

    /// <summary>
    /// All effects (ie shroud) will inherit off this Interface
    /// </summary>
    interface IMagicEffect
    {

    }


   

    #endregion

}
