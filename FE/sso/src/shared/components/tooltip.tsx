import React, { useEffect } from "react";
import { createPortal } from "react-dom";

function Tooltip(props: {
  display: JSX.Element;
  info: JSX.Element;
  position: Exclude<AppPosition, "center-center">;
}) {
  const { display, info, position } = props;

  const tooltipAnchorId = `tooltip-anchor-${new Date().getTime()}`;
  const tooltipId = `tooltip-${new Date().getTime()}`;

  useEffect(() => {
    const tt = document.getElementById(tooltipAnchorId);
    const mouseoverHandler = () => {
      const anchorRect = tt!.getBoundingClientRect();

      const tar = document.getElementById(tooltipId);
      if (!tar) return;
      const tarRect = tar!.getBoundingClientRect();
      console.log(tarRect);

      const posArr = position.split("-");
      const pos = { x: 0, y: 0 };

      switch (posArr[0]) {
        case "top":
          pos.y = anchorRect.y - 10;
          break;
        case "center":
          pos.y = anchorRect.y + anchorRect.height / 2 - tarRect.height / 2;
          break;
        case "bottom":
          pos.y = anchorRect.y + anchorRect.height + 10;
          break;
        default:
          break;
      }

      switch (posArr[1]) {
        case "left":
          pos.x = anchorRect.x - 10 - tarRect.width;
          break;
        case "center":
          pos.x = anchorRect.x + anchorRect.width / 2 - tarRect.width / 2;
          break;
        case "right":
          pos.x = anchorRect.x + anchorRect.width + 10;
          break;
        default:
          break;
      }

      tar.style.left = `${pos.x}px`;
      tar.style.top = `${pos.y}px`;
      tar.classList.remove("invisible");
    };
    const mouseoutHandler = () => {
      const tar = document.getElementById(tooltipId);
      tar?.classList.add("invisible");
    };
    tt?.addEventListener("mouseover", mouseoverHandler);
    tt?.addEventListener("mouseout", mouseoutHandler);

    return () => tt?.removeEventListener("mouseover", mouseoverHandler);
  });

  return (
    <>
      <span id={tooltipAnchorId}>{display}</span>
      {createPortal(
        <div
          id={tooltipId}
          role="tooltip"
          style={{ top: 0 }}
          className="absolute z-10 invisible inline-block px-3 py-2 transition-opacity duration-300 bg-gray-900 rounded-lg shadow-sm tooltip text-sm dark:text-slate-500 dark:bg-gray-700 max-w-64"
        >
          {info}
        </div>,
        document.querySelector("body")!
      )}
    </>
  );
}

export default Tooltip;
