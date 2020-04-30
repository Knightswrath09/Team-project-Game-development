using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject
{
    class Level
    {
        //private field storing the level number
        private int levelNum;
        //this public property can be used to read-only the level number
        public int LevelNum
        {
            get { return levelNum; }
        }

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

        //so we know which projectiles to fire in this level. all of these include green as well
        public enum ProjectileTypes
        {
            kRed_Only = 0,
            kRed_And_Blue,
            kRBP
        }

        private ProjectileTypes typesOfProjectiles;
        public ProjectileTypes TypesOfProjectiles
        {
            get { return typesOfProjectiles; }
        }

        //second between fires
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
