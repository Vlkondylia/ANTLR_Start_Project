using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;

namespace ANTLR_Start_Project
{
    class ASTGenerator : firstBaseVisitor<int>
    {
        private Stack<ASTElement> m_parents = new Stack<ASTElement>();
        public override int VisitExpr_MULDIV(firstParser.Expr_MULDIVContext context)
        {
            if (context.op.Type == firstParser.MULT)
            {
                CASTMultiplication newNode =new CASTMultiplication(nodeType.NT_MULTIPLICATION,m_parents.Peek(),2);
                m_parents.Push(newNode);
            }
            else if (context.op.Type == firstParser.DIV)
            {
                CASTDivision newNode = new CASTDivision(nodeType.NT_MULTIPLICATION, m_parents.Peek(), 2);
                m_parents.Push(newNode);
            }

            base.VisitExpr_MULDIV(context);
            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_PLUSMINUS(firstParser.Expr_PLUSMINUSContext context)
        {
            if (context.op.Type == firstParser.MINUS)
            {
                CASTSubtraction newNode = new CASTSubtraction(nodeType.NT_SUBTRACTION,m_parents.Peek(),2);
                m_parents.Push(newNode);
            }
            else if (context.op.Type==firstParser.PLUS)
            {
                CASTAddition newNode= new CASTAddition(nodeType.NT_ADDITION,m_parents.Peek(),2);
                m_parents.Push(newNode);
            }
            base.VisitExpr_PLUSMINUS(context);

            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_ASSIGNMENT(firstParser.Expr_ASSIGNMENTContext context)
        {
            CASTAssignment newNode =new CASTAssignment(nodeType.NT_ASSIGNMENT,m_parents.Peek(),2);
            m_parents.Push(newNode);

            base.VisitExpr_ASSIGNMENT(context);

            m_parents.Pop();
            return 0;

        }

        public override int VisitCompileUnit(firstParser.CompileUnitContext context)
        {
            CASTCompileUnit newNode = new CASTCompileUnit(nodeType.NT_COMPILEUNIT,m_parents.Peek(),1);
            m_parents.Push(newNode);
            base.VisitCompileUnit(context);
            m_parents.Pop();
            return 0;
        }

        public override int VisitExpr_PARENTHESIZED(firstParser.Expr_PARENTHESIZEDContext context)
        {
            
            return base.VisitExpr_PARENTHESIZED(context);
        }

        public override int VisitTerminal(ITerminalNode node)
        {
            if (firstParser.NUMBER.ToString()==node.GetText())
            {
                CASTNUMBER newNode = new CASTNUMBER(nodeType.NT_IDENTIFIER,m_parents.Pop(),2);
            }
            if (firstParser.IDENTIFIER.ToString() == node.GetText())
            {
                CASTIDENTIFIER newNode = new CASTIDENTIFIER(nodeType.NT_IDENTIFIER, m_parents.Pop(), 2);
            }
            base.VisitTerminal(node);
            return 0;
        }
    }
}
