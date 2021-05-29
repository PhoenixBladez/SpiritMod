using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
    public class MeteoriteOrbitter : ModProjectile
    {
        public bool pickedDust = false;
        public int chosenDust = 1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteorite Energy");
        }
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.hide = true;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
        }
        public override void AI()
        {

        }
    }
}