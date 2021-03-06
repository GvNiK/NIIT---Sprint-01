using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ProjectileFactory
{
	public IProjectile Create(ProjectileType type, GameObject obj,
		ProjectilePool pool)
	{
		switch(type)
		{
			case ProjectileType.Damage:
				return new DamageAmmoProjectile(obj.transform);
			case ProjectileType.Explosive:
				return new ExplosiveAmmoProjectile(obj.transform, pool);
			case ProjectileType.SubExplosion:
				return new Explosion(obj.transform);
			case ProjectileType.Guard:
				return new GuardProjectile(obj.transform);
		}

		return null;
	}
}
