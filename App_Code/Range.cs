using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


public class RangeService
{
    public static bool IsValidRangeString(string range)
    {
        Regex regexVal = new Regex(@"(\d|\d-\d)(,(\d|\d-\d))*");
        return regexVal.IsMatch(range.Trim());
    }

    private static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    public static IEnumerable<Range> ParseRange(string range)
    {
        range = range.Trim();
        if (!IsValidRangeString(range)) {
            return null;
        }
        List<Range> rangeList = new List<Range>();
        string[] parts = range.Split(',');
        foreach (string partG in parts) {
            string part = partG.Trim();
            if (part.Length == 0) {
                continue;
            }
            string[] rangeComponents = part.Split('-');
            int low, high;
            if (rangeComponents.Length == 1) {
                low = high = int.Parse(part);
            } else {
                low = int.Parse(rangeComponents[0]);
                high = int.Parse(rangeComponents[1]);
            }
            if (high < low) {
                Swap(ref low, ref high);
            }
            rangeList.Add(new Range(low, high));
        }
        return rangeList;
    }
}

/// <summary>
/// Range representation
/// </summary>
public class Range : IEnumerable
{
    public int Low { get; private set; }
    public int High { get; private set; }

	public Range(int low, int high)
	{
        Low = low;
        High = high;
	}

    public int Count()
    {
        return High - Low + 1;
    }

    public IEnumerator GetEnumerator()
    {
        return new RangeEnumerator(Low, High);
    }
}

public class RangeEnumerator : IEnumerator
{
    private int m_low;
    private int m_high;
    private int m_actual;

    public RangeEnumerator(int low, int high)
    {
        m_low = low;
        m_high = high;
        m_actual = m_low - 1;
    }

    public bool MoveNext()
    {
        if (m_actual < m_high) {
            m_actual++;
            return true;
        }
        return false;
    }

    public object Current
    {
        get
        {
            if (m_actual < m_low || m_actual > m_high) {
                throw new InvalidOperationException();
            }
            return m_actual;
        }
    }

    public void Reset()
    {
        m_actual = m_low - 1;
    }
}
