import type { Config } from 'tailwindcss';

const config: Config = {
  content: ['./src/**/*.{html,ts}'],
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        primary: '#13ec80',
        'background-light': '#f6f8f7',
        'background-dark': '#102219'
      },
      fontFamily: {
        display: ['Manrope', 'sans-serif']
      },
      borderRadius: {
        DEFAULT: '0.25rem',
        lg: '0.5rem',
        xl: '0.75rem',
        full: '9999px'
      }
    }
  },
  plugins: []
};

export default config;
