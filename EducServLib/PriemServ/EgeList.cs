using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace EducServLib
{
    public class EgeList
    {
        private SortedList<EgeCertificateClass, List<EgeMarkCert>> slEge;

        //constructor
        public EgeList()
        {
            slEge = new SortedList<EgeCertificateClass, List<EgeMarkCert>>(new EgeCertificateComparer());
        }

        //property for collection
        public SortedList<EgeCertificateClass, List<EgeMarkCert>> EGEs
        {
            get
            {
                return this.slEge;
            }
        }
        
        //add element
        public void Add(EgeMarkCert mark)
        {
            EgeCertificateClass cert = new EgeCertificateClass(mark.Doc, mark.Tipograf);

            //string doc = mark.Doc;

            if (slEge.ContainsKey(cert))
            {
                slEge[cert].Add(mark);                
            }
            else
            {
                List<EgeMarkCert> list = new List<EgeMarkCert>();
                list.Add(mark);
                slEge.Add(cert, list);                
            }
            return;
        }        
    }

    public class EgeCertificateComparer : IComparer<EgeCertificateClass>
    {
        public int Compare(EgeCertificateClass ec1, EgeCertificateClass ec2)
        {
            if (ec1.Doc.CompareTo(ec2.Doc) == 0)
                return ec1.Tipograf.CompareTo(ec2.Tipograf);
            else
                return ec1.Doc.CompareTo(ec2.Doc);
        }
    }

    public class EgeCertificateClass
    {
        public string Doc;
        public string Tipograf;

        public EgeCertificateClass(string doc, string tipograf)
        {
            this.Doc = doc;
            this.Tipograf = tipograf;
        }

        public override bool Equals(object obj)
        {
            if (obj is EgeCertificateClass)
            {
                EgeCertificateClass cert = (obj as EgeCertificateClass);
                return (cert.Doc.CompareTo(this.Doc) == 0 && cert.Tipograf.CompareTo(this.Tipograf)==0);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Doc.Length + Tipograf.Length;
        }        
    }

    //struct to keep ege mark
    public struct EgeMarkCert
    {
        public EgeMarkCert(string subj, string val, string doc, string tipograf)
        {
            this.Doc = doc;
            this.Value = val;
            this.Subject = subj;
            this.Tipograf = tipograf;
        }

        public string Value;
        public string Doc;
        public string Subject;
        public string Tipograf;
    }

}
