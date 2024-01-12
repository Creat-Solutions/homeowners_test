using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public delegate void ApplicationDelegate();

public class Application : Page
{
    private List<string> steps = null;
    private Control currentControl = null;
    protected PlaceHolder placeHolder = null;
    protected Literal title = null;
    private string m_varName = null;
    private bool stepChange = false;
    private ApplicationDelegate vButtons = null;
    private HashSet<string> m_variables = new HashSet<string>();

    protected int StepIndex
    {
        get
        {
            if (Session[m_varName] == null)
                return 0;
            return (int)Session[m_varName];
        }
        set
        {
            Session[m_varName] = value;
        }
    }

    protected int NumberSteps
    {
        get
        {
            return steps.Count;
        }
    }

    public Application(string cookiename) :
        this(cookiename, null, null, null)
    {
    }

    public Application(string cookiename, ApplicationDelegate validateFn) :
        this(cookiename, validateFn, null, null)
    {
    }

    public Application(string cookiename, ApplicationDelegate validateFn, PlaceHolder holder) :
        this(cookiename, validateFn, holder, null)
    {
    }

	public Application(string cookiename, ApplicationDelegate validateFn, PlaceHolder holder, Literal title)
	{
        steps = new List<string>();
        m_varName = cookiename;
        vButtons = validateFn;
        placeHolder = holder;
        this.title = title;
	}

    public void RegisterSessionVariable(string key)
    {
        m_variables.Add(key);
    }

    public object GetSessionVariable(string key)
    {
        if (m_variables.Contains(key)) {
            return Session[key];
        }
        return null;
    }

    public void ChangeSessionVariable(string key, object value)
    {
        if (m_variables.Contains(key)) {
            Session[key] = value;
        }
    }


    protected void LoadStep(bool loadControl)
    {
        if (vButtons != null)
            vButtons();
        currentControl = LoadApplicationControl(StepIndex);
        currentControl.ID = "currentStep";
        if (placeHolder != null) {
            if (stepChange) {
                placeHolder.Controls.Clear();
            }
            try {
                placeHolder.Controls.Add(currentControl);
            } catch {
            }
        }
        try {
            if (loadControl)
                ((IApplicationStep)currentControl).LoadStep();
            if (title != null)
                title.Text = ((IApplicationStep)currentControl).Title();
        } catch {
        }
    }

    protected void PreviousStep(ApplicationDelegate fn)
    {
        if (StepIndex > 0) {
            stepChange = true;
            StepIndex = StepIndex - 1;
            if (vButtons != null)
                vButtons();
            if (fn != null)
                fn();
            LoadStep(true);
        }
    }

    protected void LoadStep(int index, ApplicationDelegate fn)
    {
        StepIndex = index;
        stepChange = true;
        if (vButtons != null)
            vButtons();
        if (fn != null)
            fn();
        LoadStep(true);
    }

    protected bool SaveCurrentStep()
    {
        bool state = false;
        try {
            state = ((IApplicationStep)currentControl).SaveStep();
        } catch {
            state = false;
        }
        return state;
    }

    protected void NextStep(ApplicationDelegate fn)
    {
        bool state = false;
        try {
            state = ((IApplicationStep)currentControl).SaveStep();
        } catch {
            return;
        }
        if (!state)
            return;
        if (StepIndex < NumberSteps - 1) {
            stepChange = true;
            StepIndex = StepIndex + 1;
            if (vButtons != null)
                vButtons();
            if (fn != null)
                fn();
            LoadStep(true);
        }
    }

    protected void ChangeValidateButtons(ApplicationDelegate validateMethod)
    {
        vButtons = validateMethod;
    }

    protected void AddStep(string step)
    {
        steps.Add(step);
    }

    protected Control LoadApplicationControl(int index)
    {
        if (index < 0)
            return null;
        string cname = steps[index];
        return base.LoadControl(cname);
    }

    protected Control LoadApplicationControl(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;
        int index = steps.FindIndex(m => m == name);
        if (index >= 0)
            return LoadApplicationControl(index);
        return null;
    }

    protected void CancelApplication()
    {
        Session[m_varName] = null;
        foreach (string key in m_variables) {
            Session[key] = null;
        }
    }
}
