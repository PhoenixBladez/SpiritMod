using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
namespace SpiritMod.Tide
{
    public class TidePlayer : ModPlayer
    {
        public bool createdProjectiles;
        public override void PostUpdate()
        {

            if (!TideWorld.TheTide)
            {
                TideWorld.TidePoints = 0;
                TideWorld.EnemyKills = 0;
                createdProjectiles = false;
            }
            else if (TideWorld.TidePoints >= 100)
            {
                TideWorld.TidePoints = 0;
                TideWorld.EnemyKills = 0;
                Main.NewText("The tide has ended.", 145, 0, 255);
                TideWorld.TheTide = false;
            }
        }
    }
}
