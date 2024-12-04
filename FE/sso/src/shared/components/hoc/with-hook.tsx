import React from "react";

function withHook<T extends React.PropsWithChildren>(
  Component: React.ComponentType<T>
) {
  return function WrappedComponent(props: T) {
    return <Component {...props} />;
  };
}

export default withHook;
