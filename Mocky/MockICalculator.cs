using Mocky;
using System;

class MockICalculator : ICalculator {

    private MockMethod[] obj;
    public MockICalculator(MockMethod[] obj) {
        this.obj = obj;
    }

    public int Add(int a, int b) {
        if (a==5 && b==3) {
            return 8;
        }
        return 0;
    }

    public int Div(int a, int b) {
        throw new NotImplementedException();
    }

    public int Mul(int a, int b) {
        throw new NotImplementedException();
    }

    public int Sub(int a, int b) {
        throw new NotImplementedException();
    }
}
