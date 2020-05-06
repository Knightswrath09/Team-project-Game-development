using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject
{
    //stores values that differentiate one level from another
    class Level
    {
        //private field storing the level number
        private int levelNum;
        //this public property can be used to read-only the level number
        //will be displayed on the screen 
        public int LevelNum
        {
            get { return levelNum; }
        }

        //projectiles will move at different speeds depending on the level
        private float projectileSpeed;
        public float ProjectileSpeed
        {
            get { return projectileSpeed; }
        }

        //stores total projectiles to be fired this level
        private int totalProjectiles;
        //read-only the total number of projectiles to be fired in this level
        public int TotalProjectiles
        {
            get { return totalProjectiles; }
        }

        //stores number of projectiles fired so far
        private int firedProjectiles;
        public int FiredProjectiles
        {
            get { return firedProjectiles; }
            set { firedProjectiles = value; }
        }

        //represents the assortments of projectiles that can be present in a level
        //all of these include green as well
        //ex: level one only has red and green projectiles, while level four has projectiles of all four colors
        public enum ProjectileTypes
        {
            kRed_Only = 0,
            kRed_And_Blue,
            kRBP
        }

        //stores the types of projectiles that will be fired in this level
        private ProjectileTypes typesOfProjectiles;
        public ProjectileTypes TypesOfProjectiles
        {
            get { return typesOfProjectiles; }
        }

        //number of seconds between fires
        //higher levels have higher firing frequencies
        private int fireFreq;
        public int FireFreq
        {
            get { return fireFreq; }
        }


        //constructor
        public Level(int newLevelNum, int newTotalProjectiles, ProjectileTypes newTypesProj, int newFireFreq, float newSpeed)
        {
            levelNum = newLevelNum;
            totalProjectiles = newTotalProjectiles;
            typesOfProjectiles = newTypesProj;
            firedProjectiles = 0;
            fireFreq = newFireFreq;
            projectileSpeed = newSpeed;
        }
    }
}
