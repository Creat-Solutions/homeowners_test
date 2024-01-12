using System;
using System.Collections.Generic;
using System.Text;

public interface IApplicationStep
{
    bool SaveStep();
    void LoadStep();
    string Title();
}
