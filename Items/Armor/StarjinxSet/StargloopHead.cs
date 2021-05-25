using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Stargoop;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using System;

namespace SpiritMod.Items.Armor.StarjinxSet
{
	[AutoloadEquip(EquipType.Head)]
    public class StargloopHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stargloop Head");
			Tooltip.SetDefault("'Head beyond the clouds'");
		}
		public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.value = Item.sellPrice(gold : 2);
            item.rare = 4;
			item.vanity = true;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawHair = drawAltHair = false;
		public override bool DrawHead() => false;
		public override void UpdateVanity(Player player, EquipType type)
		{
			float dir = Main.rand.NextFloat(-2f, -1.14f);

			float midMultiplier = 1.4f - Math.Abs(dir + 1.57f) * 1.5f;

			Vector2 velocity = dir.ToRotationVector2() * Main.rand.NextFloat(1f, 3f) * midMultiplier;

			Vector2 center = player.MountedCenter;

			Dust dust = Dust.NewDustPerfect(center - player.velocity + new Vector2(0, -10),
				ModContent.DustType<FriendlyStargoopDust>(), velocity + player.velocity * 0.5f, Scale: Main.rand.NextFloat(1.4f, 1.8f));


			base.UpdateVanity(player, type);
		}
	}
}
