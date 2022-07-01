namespace Quest.Systems.Skills;

public enum SkillType
{
	skill_mining
}

public static class SkillHelpers
{
	public static Dictionary<SkillType, string> SkillIcons { get; set; } = new()
	{
		{ SkillType.skill_mining, "/ui/skills/mining.png" }
	};
}
