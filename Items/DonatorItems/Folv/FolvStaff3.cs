using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
    [AutoloadEquip(EquipType.Balloon)]
    public class FolvStaff3 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Staff of Protection");
			Tooltip.SetDefault("Increases maximum mana by 30, and magic damage by 8% \nGrants immunity to a multitude of debuffs \nReduces damage taken by 8% when under half health and grants immunity to knockback\n~ Donator Item~");
		}

        public override void SetDefaults()
        {
            item.width = 60;
            item.height = 60;
            item.rare = 7;
            item.value = 95000;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            player.statManaMax2 += 30;
            player.magicDamage *= 1.08f;
            {
                player.buffImmune[BuffID.Bleeding] = true;
                player.buffImmune[BuffID.BrokenArmor] = true;
                player.buffImmune[BuffID.Darkness] = true;
                player.buffImmune[BuffID.Poisoned] = true;
                player.buffImmune[BuffID.Burning] = true;
                player.buffImmune[BuffID.Silenced] = true;
                player.buffImmune[BuffID.Slow] = true;
                player.buffImmune[BuffID.Confused] = true;
                player.buffImmune[BuffID.Cursed] = true;
                player.buffImmune[BuffID.Weak] = true;
                player.buffImmune[BuffID.Chilled] = true;
            }
            if (player.statLife < player.statLifeMax2 / 2)
            {
                player.endurance = 0.08f;
                int dust = Dust.NewDust(player.position, player.width, player.height, 187);
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, "FolvStaff2", 1);
            recipe.AddIngredient(null, "IcyEssence", 5);
            recipe.AddIngredient(ItemID.AnkhCharm, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
