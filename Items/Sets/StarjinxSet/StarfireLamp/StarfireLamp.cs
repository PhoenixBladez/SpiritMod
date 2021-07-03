using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
    public class StarfireLamp : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 60;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.magic = true;
            item.width = 36;
            item.height = 40;
            item.useTime = 10;
            item.useAnimation = 40;
			item.reuseDelay = 20;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.shoot = ModContent.ProjectileType<StarfireProj>();
            item.shootSpeed = 16f;
            item.knockBack = 3f;
            item.autoReuse = true;
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item45;
            item.value = Item.sellPrice(silver: 55);
            item.useTurn = false;
            item.mana = 15;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Lantern");
            Tooltip.SetDefault("Emits embers of cosmic energy\nRight click to illuminate an enemy\nEmbers lock on to illuminated enemies");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			position.Y -= 60;
			Vector2 vel = Vector2.Normalize(Main.MouseWorld - position).RotatedByRandom(MathHelper.Pi / 16) * item.shootSpeed;
			speedX = vel.X;
			speedY = vel.Y;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "Starjinx", 6);
            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
