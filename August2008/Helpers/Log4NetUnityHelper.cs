using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System.Collections.Generic;

namespace August2008.Common
{
    public class BuildTrackingExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<BuildTrackingStrategy>(UnityBuildStage.TypeMapping);
        }
        public static IBuildTrackingPolicy GetPolicy(IBuilderContext context)
        {
            return context.Policies.Get<IBuildTrackingPolicy>(context.BuildKey, true);
        }
        public static IBuildTrackingPolicy SetPolicy(IBuilderContext context)
        {
            IBuildTrackingPolicy policy = new BuildTrackingPolicy();
            context.Policies.SetDefault(policy);
            return policy;
        }
    }
    public class BuildTrackingStrategy : BuilderStrategy
    {

        public override void PreBuildUp(IBuilderContext context)
        {
            var policy = BuildTrackingExtension.GetPolicy(context) ?? BuildTrackingExtension.SetPolicy(context);
            policy.BuildKeys.Push(context.BuildKey);
        }
        public override void PostBuildUp(IBuilderContext context)
        {
            IBuildTrackingPolicy policy = BuildTrackingExtension.GetPolicy(context);
            if ((policy != null) && (policy.BuildKeys.Count > 0))
            {
                policy.BuildKeys.Pop();
            }
        }
    }
    public interface IBuildTrackingPolicy : IBuilderPolicy
    {
        Stack<object> BuildKeys { get; }
    }
    public class BuildTrackingPolicy : IBuildTrackingPolicy
    {

        public BuildTrackingPolicy()
        {
            BuildKeys = new Stack<object>();
        }
        public Stack<object> BuildKeys 
        { 
            get; 
            private set; 
        }
    }
    public class LogCreationExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.AddNew<LogCreationStrategy>(UnityBuildStage.PreCreation);
        }
    }
    public class LogCreationStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Type typeToBuild = context.BuildKey.Type;
            if (typeof(log4net.ILog).Equals(typeToBuild))
            {

                if (context.Policies.Get<IBuildPlanPolicy>(context.BuildKey) == null)
                {
                    Type typeForLog = LogCreationStrategy.GetLogType(context);
                    IBuildPlanPolicy policy = new LogBuildPlanPolicy(typeForLog);
                    context.Policies.Set<IBuildPlanPolicy>(policy, context.BuildKey);

                    IsPolicySet = true;
                }
            }
        }
        public override void PostBuildUp(IBuilderContext context)
        {
            if (IsPolicySet)
            {
                context.Policies.Clear<IBuildPlanPolicy>(context.BuildKey);
                IsPolicySet = false;
            }
        }
        private static Type GetLogType(IBuilderContext context)
        {
            Type logType = null;
            IBuildTrackingPolicy buildTrackingPolicy = BuildTrackingExtension.GetPolicy(context);
            if ((buildTrackingPolicy != null) && (buildTrackingPolicy.BuildKeys.Count >= 2))
            {
                logType = ((NamedTypeBuildKey)buildTrackingPolicy.BuildKeys.ElementAt(1)).Type;
            }
            else
            {
                StackTrace stackTrace = new StackTrace();
                //first two are in the log creation strategy, can skip over them
                for (int i = 2; i < stackTrace.FrameCount; i++)
                {
                    StackFrame frame = stackTrace.GetFrame(i);
                    logType = frame.GetMethod().DeclaringType;
                    if (!logType.FullName.StartsWith("Microsoft.Practices"))
                    {
                        break;
                    }
                }
            }
            return logType;
        }
        public bool IsPolicySet
        {
            get;
            private set;
        }
    }
    public class LogBuildPlanPolicy : IBuildPlanPolicy
    {

        public LogBuildPlanPolicy(Type logType)
        {
            LogType = logType;
        }
        public void BuildUp(IBuilderContext context)
        {
            if (context.Existing == null)
            {
                var log = log4net.LogManager.GetLogger(LogType);
                context.Existing = log;
            }
        }
        public Type LogType 
        { 
            get; 
            private set; 
        }
    }
}
