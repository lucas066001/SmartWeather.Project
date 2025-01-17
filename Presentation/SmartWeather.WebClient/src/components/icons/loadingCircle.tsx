
interface ILoadingCircleProps {
  color: string;
  className?: string;
}

function LoadingCircle({ className, color }: ILoadingCircleProps) {
  return (
    <svg
      className={className}
      width="24" // Set width to match Notifications SVG
      height="24" // Set height to match Notifications SVG
      viewBox="0 0 200 200" // Keep the original viewBox to maintain the circle's proportions
      xmlns="http://www.w3.org/2000/svg"
    >
      <radialGradient id="a12" cx=".66" fx=".66" cy=".3125" fy=".3125" gradientTransform="scale(1.5)">
        <stop offset="0" stopColor={color} />
        <stop offset=".3" stopColor={color} stopOpacity=".9" />
        <stop offset=".6" stopColor={color} stopOpacity=".6" />
        <stop offset=".8" stopColor={color} stopOpacity=".3" />
        <stop offset="1" stopColor={color} stopOpacity="0" />
      </radialGradient>
      <circle
        transformOrigin="center"
        fill="none"
        stroke="url(#a12)"
        strokeWidth="30"
        strokeLinecap="round"
        strokeDasharray="200 1000"
        strokeDashoffset="0"
        cx="100"
        cy="100"
        r="70"
      >
        <animateTransform
          type="rotate"
          attributeName="transform"
          calcMode="spline"
          dur="2"
          values="360;0"
          keyTimes="0;1"
          keySplines="0 0 1 1"
          repeatCount="indefinite"
        />
      </circle>
      <circle
        transformOrigin="center"
        fill="none"
        opacity=".2"
        stroke={color}
        strokeWidth="30"
        strokeLinecap="round"
        cx="100"
        cy="100"
        r="70"
      />
    </svg>
  );
}

export default LoadingCircle;
