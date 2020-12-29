using System;

namespace TestClassLib
{
    public class Class1
    {
        public class Class2 : iClass
        {
            public String s;
            public String s2;
            private String s3;
            public override void Method()
            {
                
            }

            private void Method1()
            {

            }

            private void Method2()
            {

            }
            public class Class3
            {
                public String s;
                public String s2;
                private String s3;

                private void Method1()
                {

                }
                private void Method2()
                {

                }
                private void Method3()
                {

                }
                public void Method4()
                {

                }
                public class Class4 : iClass
                {
                    private void Method3()
                    {

                    }
                }

                public class Class5 : iClass
                {
                    private String s;
                    private String s2;
                    private String s3;
                }
            }

        }

        public class iClass
        {
            public String s;
            public int x;
            public int y;
            public virtual void Method()
            {

            }
        }
    }
}
