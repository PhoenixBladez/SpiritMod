using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet.BismiteArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class BismiteHelmet : SpiritSetbonusHead
	{
		public override string SetDisplayName => "Bismite Helmet";
		public override string SetTooltip => "4% increased movement speed";
		public override bool SetBody(Item body) => body.type == ModContent.ItemType<BismiteChestplate>();
		public override bool SetLegs(Item legs) => legs.type == ModContent.ItemType<BismiteLeggings>();
		public override string SetbonusText
			=> "Not getting hit builds up stacks of Virulence\n"
			+ "Virulence charges up every 10 seconds\n"
			+ "Striking while Virulence is charged releases a toxic explosion\n"
			+ "Getting hit depletes Virulence entirely";
		public override SpiritPlayerEffect SetbonusEffect => new BismiteSetbonusEffect();

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.buyPrice(silver: 10);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawAltHair = true;

		public override void UpdateEquip(Player player) => player.moveSpeed += .04f;

		public override void ArmorSetShadows(Player player)
		{
			if (player.GetSpiritPlayer().setbonus is BismiteSetbonusEffect setbonus && setbonus.virulence <= 0) {
				player.armorEffectDrawShadow = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	public class BismiteSetbonusEffect : SpiritPlayerEffect
	{
		public float virulence = 600f;
		public int projDamage = 15;

		public override void EffectRemoved(Player player)
			=> virulence = 600f;

		public override void PlayerPostUpdate(Player player)
		{
			if (player.HasBuff(ModContent.BuffType<VirulenceCooldown>()) || virulence >= 0)
				virulence--;
			if (virulence == 0f)
				SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(25, 1));
		}
		public override void PlayerHurt(Player player, bool pvp, bool quiet, double damage, int hitDirection, bool crit) => virulence = 600f;

		public override void PlayerOnHitNPC(Player player, Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (virulence <= 0f) {
				Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<VirulenceExplosion>(), 25, 8, Main.myPlayer);
				virulence = 600f;
				player.AddBuff(ModContent.BuffType<VirulenceCooldown>(), 140);
			}
		}

		public override void PlayerOnHitNPCWithProj(Player player, Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if (virulence <= 0f) {
				Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<VirulenceExplosion>(), 25, 8, Main.myPlayer);
				virulence = 600f;
				player.AddBuff(ModContent.BuffType<VirulenceCooldown>(), 140);
			}
		}

		public override void PlayerDrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			var player = drawInfo.drawPlayer;
			if (virulence <= 0 && Main.rand.NextBool(2)) {
				for (int index1 = 0; index1 < 3; ++index1) {
					int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Plantera_Green, 0, 0, 167, default, Main.rand.NextFloat(.5f, 1.32f));
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
	          		Main.dust[dust].customData = player;
					Main.playerDrawDust.Add(dust);
				}
			}
		}
	}
}
