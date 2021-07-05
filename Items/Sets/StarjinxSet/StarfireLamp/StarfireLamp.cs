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
using System.Linq;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
    public class StarfireLamp : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 50;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.magic = true;
            item.width = 36;
            item.height = 40;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.shoot = ModContent.ProjectileType<StarfireProj>();
            item.shootSpeed = 24f;
            item.knockBack = 3f;
            item.autoReuse = true;
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item45.WithPitchVariance(0.2f).WithVolume(0.5f);
            item.value = Item.sellPrice(silver: 55);
            item.useTurn = false;
            item.mana = 15;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Lantern");
            Tooltip.SetDefault("Emits embers of cosmic energy\nRight click to illuminate an enemy\nEmbers lock on to illuminated enemies");
        }

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
				NPC mousehovernpc = null; //see if an npc is intersecting the mouse
				foreach (NPC npc in Main.npc.Where(x => x.active && x.CanBeChasedBy(player) && x != null))
				{
					Rectangle hitbox = npc.Hitbox;
					hitbox.Inflate(40, 40);
					if (hitbox.Contains(Main.MouseWorld.ToPoint()))
						mousehovernpc = npc;
				}

				if (mousehovernpc == null)
					return false;

				return true;
			}
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			StarfireLampPlayer starfireLampPlayer = player.GetModPlayer<StarfireLampPlayer>();
			starfireLampPlayer.TwinkleTime = StarfireLampPlayer.MaxTwinkleTime;
			if (player.altFunctionUse == 2)
			{
				NPC mousehovernpc = null;
				foreach (NPC npc in Main.npc.Where(x => x.active && x.CanBeChasedBy(player) && x != null)) //iterate through npcs and filter out ones that shouldnt be targetted
				{
					Rectangle hitbox = npc.Hitbox;
					hitbox.Inflate(40, 40);
					if (hitbox.Contains(Main.MouseWorld.ToPoint()))
						mousehovernpc = npc;
				}
				if (mousehovernpc == null)
					return false;

				starfireLampPlayer.LampTargetNPC = mousehovernpc;
				starfireLampPlayer.LampTargetTime = StarfireLampPlayer.MaxTargetTime;
				return false;
			}
			position.Y -= 60;
			Vector2 vel = Vector2.Normalize(Main.MouseWorld - position).RotatedByRandom(MathHelper.Pi / 20) * item.shootSpeed;
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
