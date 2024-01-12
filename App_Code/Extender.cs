using ME.Admon;
using Protec.Authentication.Membership;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

/// <summary>
/// Summary description for Extender
/// </summary>
public static class Extender
{
	public static List<T> ToUniqueList<T>(this IEnumerable<T> list) where T : AbstractLocation
	{
		Dictionary<int, T> unique = new Dictionary<int, T>();
		foreach (var member in list)
		{
			if (unique.ContainsKey(member.ID))
			{
				continue;
			}

			unique[member.ID] = member;
		}

		return unique.Values.ToList();
	}

	public static UserInformation GetUserInformation(this Page page)
	{
		return page.User.Identity as UserInformation;
	}
}