using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Stargoop;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class NebulaGoopDust : StargoopDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;

            Attach(SpiritMod.Metaballs.NebulaLayer);

        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale *= 0.95f;
            dust.velocity *= 0.9f;
            dust.rotation += 0.1f;

            return base.Update(dust);
        }
    }
}